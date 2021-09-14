using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VeryNaiveDatalog.Test
{
    public class EvaluatorTests
    {
        [Test]
        public void Evaluator_Ancestor_Example()
        {
            // Rules
            var r0 = new Rule(new Atom("ancestor", new Variable("x"), new Variable("y")),
                new Atom("parent", new Variable("x"), new Variable("y")));

            var r1 = new Rule(new Atom("ancestor", new Variable("x"), new Variable("y")),
                new Atom("ancestor", new Variable("x"), new Variable("z")),
                new Atom("parent", new Variable("z"), new Variable("y")));

            // Facts
            var f0 = new Atom("parent", new Symbol("Homer"), new Symbol("Lisa"));
            var f1 = new Atom("parent", new Symbol("Homer"), new Symbol("Bart"));
            var f2 = new Atom("parent", new Symbol("Grampa"), new Symbol("Homer"));
            
            // Query
            var q = new Atom("ancestor", new Variable("x"), new Symbol("Bart"));

            // Run
            var rules = new[] {r0, r1};
            var facts = new[] {f0, f1, f2};
            
            var result = facts.Query(q, rules);

            // Assert
            var expected0 = new Substitution() {{new Variable("x"), new Symbol("Homer")}};
            var expected1 = new Substitution() {{new Variable("x"), new Symbol("Grampa")}};
            var expected = new[] {expected0, expected1};
            
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void Evaluator_Parsing_Example()
        {
            // Helpers
            var i = new Variable("i");
            var j = new Variable("j");
            var k = new Variable("k");
            Func<int, Symbol> s = i => new Symbol(i.ToString());
            Func<int, Atom> eps = i => new Atom("Epsilon", s(i), s(i));

            // Grammar
            // A ::= <epsilon> | A A | L B
            // B ::= A R
            // L ::= '('
            // R ::= ')'
            var a0 = new Rule(new Atom("A", i, j), new Atom("Epsilon", i, j));
            var a1 = new Rule(new Atom("A", i, j), new Atom("A", i, k), new Atom("A", k, j));
            var a2 = new Rule(new Atom("A", i, j), new Atom("L", i, k), new Atom("B", k, j));
            var b0 = new Rule(new Atom("B", i, j), new Atom("A", i, k), new Atom("R", k, j));

            var rules = new[] { a0, a1, a2, b0 };

            IEnumerable<Atom> Lex(Char c, int i)
            {
                yield return eps(i);
                if ('(' == c)
                {
                    yield return new Atom("L", s(i), s(i + 1));
                }

                if (')' == c)
                {
                    yield return new Atom("R", s(i), s(i + 1));
                }
            }

            // Test case: can we find of derivation of A from
            // offset 0 to offset src.Length?
            var src = "(()())()";
            var facts = src.SelectMany(Lex);
            var query = new Atom("A", s(0), s(src.Length));
            var result = facts.Query(query, rules);

            // The resulting substitution is empty since there's no
            // variables in the query.
            Assert.AreEqual(1, result.Count());
        }
        
    }
}
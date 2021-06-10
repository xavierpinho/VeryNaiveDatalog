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
    }
}
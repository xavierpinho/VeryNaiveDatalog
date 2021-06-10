using System;
using System.Collections.Generic;
using System.Linq;

namespace VeryNaiveDatalog
{
    /// <summary>
    /// A rule (or [Horn] clause) is an expression A_0 :- A_1, ..., A_n
    /// composed of a head (A_0) and a body (A_1, ..., A_n), where A_i are atoms.
    /// A rule without body is called a fact.
    /// Examples:
    /// parent(Homer,Lisa) -- a fact expressing that Homer is Lisa's parent
    /// ancestor(x,z):-ancestor(x,y),parent(y,z) -- a rule for deducing ancestors from parents
    /// </summary>
    public class Rule
    {
        public Atom Head { get; }
        public IList<Atom> Body { get; }

        public Rule(Atom head, IEnumerable<Atom> body)
        {
            Head = head;
            Body = body.ToList();
        }
        
        public Rule(Atom head, params Atom[] body) : this(head, body.AsEnumerable()) {}

        public override bool Equals(object? obj) =>
            obj switch
            {
                Rule that => Head.Equals(that.Head) && Body.SequenceEqual(that.Body),
                _ => false
            };

        public override int GetHashCode() => HashCode.Combine(Head, Body);
        
    }
}
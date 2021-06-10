using System;
using System.Collections.Generic;
using System.Linq;

namespace VeryNaiveDatalog
{
    /// <summary>
    /// An atom is an expression p(t_0, t_1, ..., t_n) composed of
    /// a predicate name (p) and a finite list of terms (t_0, ..., t_n).
    ///
    /// Examples:
    /// parent(Homer, Lisa) -- parent is the predicate and Homer/Lisa are symbols.
    /// parent(x, Lisa)     -- x is a variable.
    /// </summary>
    public class Atom
    {
        public string Name { get; }
        
        public IList<ITerm> Terms { get; }
        
        public int Arity => Terms.Count;

        public Atom(string name, IEnumerable<ITerm> terms)
        {
            Name = name;
            Terms = terms.ToList();
        }
        
        public Atom Apply(Substitution env) => new(Name, Terms.Select(t => t.Apply(env)));

        public Atom(string name, params ITerm[] terms) : this(name, terms.AsEnumerable()) {}
        
        public override int GetHashCode() => HashCode.Combine(Name, Terms.Aggregate(0, HashCode.Combine));

        public override bool Equals(object? obj) =>
            obj switch
            {
                Atom that => Name == that.Name && Terms.SequenceEqual(that.Terms),
                _ => false
            };
    }
}
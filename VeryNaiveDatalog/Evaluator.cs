using System.Collections.Generic;
using System.Linq;

namespace VeryNaiveDatalog;

/// <summary>
/// A naive, bottom-up evaluator.
/// </summary>
public static class Evaluator
{
    // Applies a rule to a collection of atoms, returning the newly-derived atoms.
    private static IEnumerable<Atom> Apply(this Rule rule, IEnumerable<Atom> kb)
    {
        // The initial collection of bindings from which to build upon
        var seed = new[] {new Substitution()}.AsEnumerable();
            
        // Attempt to match (unify) the rule's body with the collection of atoms.
        // Returns all successful bindings.
        var matches = rule.Body.Aggregate(seed, (envs, a) => a.UnifyWith(kb, envs));
            
        // Apply the bindings accumulated in the rule's body (the premises) to the rule's head (the conclusion),
        // thus obtaining the new atoms.
        return matches.Select(rule.Head.Apply);
    }

    // Just a lifting of Rule.Apply to an IEnumerable<Rule>.
    private static IEnumerable<Atom> Apply(this IEnumerable<Rule> rules, IEnumerable<Atom> kb) => rules.SelectMany(r => r.Apply(kb)).ToHashSet();

    // Repeatedly applies rules to atoms until no more atoms are derivable.
    private static IEnumerable<Atom> Evaluate(this IEnumerable<Atom> kb, IEnumerable<Rule> rules)
    {
        var nextKb = rules.Apply(kb);
        if (nextKb.Except(kb).Any())
        {
            return kb.Union(nextKb).Evaluate(rules);
        }

        return nextKb;
    }

    public static IEnumerable<Substitution> Query(this IEnumerable<Atom> kb, Atom q, IEnumerable<Rule> rules) =>
        q.UnifyWith(kb.Evaluate(rules), new Substitution());
}
using System.Collections.Generic;
using System.Linq;

namespace VeryNaiveDatalog
{
    public static class Unification
    {
        private static bool TryUnify(Symbol s1, Symbol s2, Substitution _) => s1.Equals(s2);
        
        private static bool TryUnify(Variable v1, ITerm t2, Substitution env) =>
            env.TryGetValue(v1, out var t1) ? t1.Equals(t2) : env.TryAdd(v1, t2);

        private static bool TryUnify(ITerm t1, ITerm t2, Substitution env) =>
            (t1, t2) switch
            {
                (Symbol s1, Symbol s2) => TryUnify(s1, s2, env),
                (Variable v1, _) => TryUnify(v1, t2, env),
                (_, Variable v2) => TryUnify(v2, t1, env),
                _ => false
            };
        
        private static bool TryUnify(Atom a1, Atom a2, Substitution env)
        {
            // If their predicate names or arities are different we can't make them equal.
            if (a1.Arity != a2.Arity || a1.Name != a2.Name)
            {
                return false;
            }
            
            // Attempt to unify their terms one-by-one, building the substitution by accumulation.
            foreach (var (t1, t2) in a1.Terms.Zip(a2.Terms))
            {
                if (!TryUnify(t1, t2, env))
                {
                    return false;
                }
            }

            return true;
        }

        // Attempt to unify an atom against a collection of atoms under a given environment.
        // Returning empty means to unification was possible.
        public static IEnumerable<Substitution> UnifyWith(this Atom atom, IEnumerable<Atom> kb, Substitution env)
        {
            var a1 = atom.Apply(env);
            foreach (var a2 in kb)
            {
                var workEnv = new Substitution(env);
                if (TryUnify(a1, a2, workEnv))
                {
                    yield return workEnv;
                }
            }
        }

        public static IEnumerable<Substitution> UnifyWith(this Atom atom, IEnumerable<Atom> kb,
            IEnumerable<Substitution> envs) =>
            envs.SelectMany(env => atom.UnifyWith(kb, env));
    }
}
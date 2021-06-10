using System.Collections.Generic;

namespace VeryNaiveDatalog
{
    public class Variable : ITerm
    {
        public Variable(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ITerm Apply(Substitution env) => env.GetValueOrDefault(this, this);

        public override bool Equals(object? obj) =>
            obj switch
            {
                Variable that => Name == that.Name,
                _ => false
            };

        public override int GetHashCode() => (GetType() + Name).GetHashCode();
    }
}
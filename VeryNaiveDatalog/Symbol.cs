namespace VeryNaiveDatalog
{
    public class Symbol : ITerm
    {
        public Symbol(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public ITerm Apply(Substitution _) => this;
        
        public override bool Equals(object? obj) =>
            obj switch
            {
                Symbol that => Name == that.Name,
                _ => false
            };

        public override int GetHashCode() => (GetType() + Name).GetHashCode();
    }
}
namespace VeryNaiveDatalog
{
    /// <summary>
    ///     A term is either a symbol or a variable.
    /// </summary>
    public interface ITerm
    {
        public string Name { get; }

        ITerm Apply(Substitution env);
    }
}
namespace LanguageExt.HKT;

/// <summary>
/// Monoid trait
/// </summary>
/// <typeparam name="A">Bound type</typeparam>
public interface Monoid<A> : Semigroup<A> 
    where A : Monoid<A>
{
    /// <summary>
    /// Monoid category empty value
    /// </summary>
    public static abstract A Empty();
}

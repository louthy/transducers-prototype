namespace LanguageExt.HKT;

/// <summary>
/// Semigroup trait
/// </summary>
/// <typeparam name="A">Bound type</typeparam>
public interface Semigroup<A> where A : Semigroup<A>
{
    /// <summary>
    /// Append operation 
    /// </summary>
    /// <param name="lhs">Left hand side</param>
    /// <param name="rhs">Right hand side</param>
    /// <returns>Appended result</returns>
    public static abstract A operator+(A lhs, A rhs);
}

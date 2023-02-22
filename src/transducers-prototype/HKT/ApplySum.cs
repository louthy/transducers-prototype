namespace LanguageExt.HKT;

/// <summary>
/// Applicative trait
/// </summary>
public interface ApplySum<F> : FunctorSum<F> 
    where F : ApplySum<F>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract K<F, X, Y, A, C> Ap<X, Y, A, B, C>(
        K<F, X, Y, A, Func<B, C>> f,  
        K<F, X, Y, A, B> x);
}

/// <summary>
/// Applicative trait with fixed left type
/// </summary>
public interface ApplySum<F, X> : FunctorSum<F, X> 
    where F : ApplySum<F, X>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract K<F, X, X, A, C> Ap<A, B, C>(
        K<F, X, X, A, Func<B, C>> f,
        K<F, X, X, A, B> x);
}

/// <summary>
/// Applicative trait with fixed input type
/// </summary>
public interface ApplySum<F, X, A> : FunctorSum<F, X, A> 
    where F : ApplySum<F, X, A>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract K<F, X, X, A, C> Ap<B, C>(
        K<F, X, X, A,Func<B, C>> f,  
        K<F, X, X, A, B> x);
}

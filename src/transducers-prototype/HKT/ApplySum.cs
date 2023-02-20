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
    public static abstract K<F, Sum<X, A>, Sum<Y, C>> Ap<X, Y, A, B, C>(
        K<F, Sum<X, A>, Sum<Y, Func<B, C>>> f,  
        K<F, Sum<X, A>, Sum<Y, B>> x);
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
    public static abstract K<F, Sum<X, A>, Sum<X, C>> Ap<A, B, C>(
        K<F, Sum<X, A>, Sum<X, Func<B, C>>> f,  
        K<F, Sum<X, A>, Sum<X, B>> x);
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
    public static abstract K<F, Sum<X, A>, Sum<X, C>> Ap<B, C>(
        K<F, Sum<X, A>, Sum<X, Func<B, C>>> f,  
        K<F, Sum<X, A>, Sum<X, B>> x);
}

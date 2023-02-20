namespace LanguageExt.HKT;

/// <summary>
/// Applicative trait
/// </summary>
public interface SumApply<F> : SumFunctor<F> 
    where F : SumApply<F>
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
public interface SumApply<F, X> : SumFunctor<F, X> 
    where F : SumApply<F, X>
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
public interface SumApply<F, X, A> : SumFunctor<F, X, A> 
    where F : SumApply<F, X, A>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract K<F, Sum<X, A>, Sum<X, C>> Ap<B, C>(
        K<F, Sum<X, A>, Sum<X, Func<B, C>>> f,  
        K<F, Sum<X, A>, Sum<X, B>> x);
}

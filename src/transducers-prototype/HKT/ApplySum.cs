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
    public static abstract KArr<F, X, Y, A, C> Ap<X, Y, A, B, C>(
        KArr<F, X, Y, A, Func<B, C>> f,  
        KArr<F, X, Y, A, B> x);
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
    public static abstract KArr<F, X, X, Env, B> Ap<Env, A, B>(
        KArr<F, Env, X, Env, Func<A, B>> f,
        KArr<F, Env, X, Env, A> x);
}

/// <summary>
/// Applicative trait with fixed input type
/// </summary>
public interface ApplySum<F, Env, X> : FunctorSum<F, Env, X> 
    where F : ApplySum<F, Env, X>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract KArr<F, Env, X, Env, B> Ap<A, B>(
        KArr<F, Env, X, Env,Func<A, B>> f,  
        KArr<F, Env, X, Env, A> x);
}

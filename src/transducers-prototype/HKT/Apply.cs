namespace LanguageExt.HKT;

/// <summary>
/// Applicative trait
/// </summary>
public interface Apply<F> : Functor<F> 
    where F : Apply<F>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract KArr<F, A, C> Ap<A, B, C>(KArr<F, A, Func<B, C>> f, KArr<F, A, B> x);
}

/// <summary>
/// Applicative trait with fixed input type
/// </summary>
public interface Apply<F, A> : Functor<F, A> 
    where F : Apply<F, A>
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    public static abstract KArr<F, A, C> Ap<B, C>(KArr<F, A, Func<B, C>> f, KArr<F, A, B> x);
}

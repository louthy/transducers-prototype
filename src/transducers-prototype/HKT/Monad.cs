namespace LanguageExt.HKT;

/// <summary>
/// Monad bind trait
/// </summary>
public interface Monad<M> : Applicative<M> 
    where M : Monad<M>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, A, C> Bind<A, B, C>(KArr<M, A, B> mx, Transducer<B, KArr<M, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, A, C> Bind<A, B, C>(KArr<M, A, B> mx, Func<B, KArr<M, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, A, B> Flatten<A, B>(KArr<M, A, KArr<M, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, A, B>>());
}

/// <summary>
/// Monad bind trait with fixed input type
/// </summary>
public interface Monad<M, A> : Applicative<M, A> 
    where M : Monad<M, A>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, A, C> Bind<B, C>(KArr<M, A, B> mx, Transducer<B, KArr<M, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, A, C> Bind<B, C>(KArr<M, A, B> mx, Func<B, KArr<M, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, A, B> Flatten<B>(KArr<M, A, KArr<M, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, A, B>>());
}

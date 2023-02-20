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
    public static abstract K<M, A, C> Bind<A, B, C>(K<M, A, B> mx, Transducer<B, K<M, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, A, C> Bind<A, B, C>(K<M, A, B> mx, Func<B, K<M, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, A, B> Flatten<A, B>(K<M, A, K<M, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, A, B>>());
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
    public static abstract K<M, A, C> Bind<B, C>(K<M, A, B> mx, Transducer<B, K<M, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, A, C> Bind<B, C>(K<M, A, B> mx, Func<B, K<M, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, A, B> Flatten<B>(K<M, A, K<M, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, A, B>>());
}

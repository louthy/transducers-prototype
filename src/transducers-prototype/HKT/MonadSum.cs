namespace LanguageExt.HKT;

/// <summary>
/// Monad bind trait
/// </summary>
public interface MonadSum<M> : ApplicativeSum<M> 
    where M : MonadSum<M>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract K<M, Sum<X, A>, Sum<Z, C>> Bind<X, Y, Z, A, B, C>(
        K<M, Sum<X, A>, Sum<Y, B>> mx, 
        Transducer<B, K<M, Sum<X, A>, Sum<Z, C>>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<Z, C>> Bind<X, Y, Z, A, B, C>(
        K<M, Sum<X, A>, Sum<Y, B>> mx, 
        Func<B, K<M, Sum<X, A>, Sum<Z, C>>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<Y, B>> Flatten<X, Y, A, B>(
        K<M, Sum<X, A>, Sum<Y, K<M, Sum<X, A>, Sum<Y, B>>>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, Sum<X, A>, Sum<Y, B>>>());
}

/// <summary>
/// Monad bind trait with fixed left type
/// </summary>
public interface MonadSum<M, X> : ApplicativeSum<M, X> 
    where M : MonadSum<M, X>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract K<M, Sum<X, A>, Sum<X, C>> Bind<A, B, C>(
        K<M, Sum<X, A>, Sum<X, B>> mx, 
        Transducer<B, K<M, Sum<X, A>, Sum<X, C>>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<X, C>> Bind<A, B, C>(
        K<M, Sum<X, A>, Sum<X, B>> mx, 
        Func<B, K<M, Sum<X, A>, Sum<X, C>>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<X, B>> Flatten<A, B>(
        K<M, Sum<X, A>, Sum<X, K<M, Sum<X, A>, Sum<X, B>>>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, Sum<X, A>, Sum<X, B>>>());
}

/// <summary>
/// Monad bind trait with fixed left type and input type
/// </summary>
public interface MonadSum<M, X, A> : ApplicativeSum<M, X, A> 
    where M : MonadSum<M, X, A>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract K<M, Sum<X, A>, Sum<X, C>> Bind<B, C>(
        K<M, Sum<X, A>, Sum<X, B>> mx, 
        Transducer<B, K<M, Sum<X, A>, Sum<X, C>>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<X, C>> Bind<B, C>(
        K<M, Sum<X, A>, Sum<X, B>> mx, 
        Func<B, K<M, Sum<X, A>, Sum<X, C>>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<X, B>> Flatten<B>(
        K<M, Sum<X, A>, Sum<X, K<M, Sum<X, A>, Sum<X, B>>>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, Sum<X, A>, Sum<X, B>>>());
}

namespace LanguageExt;

public static partial class TransducerAsync
{
    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static SumTransducerAsync<X, Y, A, C> Map<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, Func<B, C> f) =>
        SumTransducerAsync.compose(m, SumTransducerAsync.mapRight<Y, B, C>(lift(f)));

    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static SumTransducerAsync<X, Y, A, C> Select<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, Func<B, C> f) =>
        SumTransducerAsync.compose(m, SumTransducerAsync.mapRight<Y, B, C>(lift(f)));

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, 
        Func<B, SumTransducerAsync<X, Y, A, C>> g) =>
        new SumBindTransducerAsync3<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, 
        TransducerAsync<B, SumTransducerAsync<X, Y, A, C>> g) =>
        new SumBindTransducerAsync1<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, 
        TransducerAsync<B, SumTransducer<X, Y, A, C>> g) =>
        new SumBindTransducerAsyncSync1<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, 
        TransducerAsync<B, Func<Sum<X, A>, Sum<Y, C>>> g) =>
        new SumBindTransducerAsync2<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, C> SelectMany<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, B> m, 
        Func<B, SumTransducerAsync<X, Y, A, C>> g) =>
        new SumBindTransducerAsync3<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, D> SelectMany<X, Y, A, B, C, D>(
        this SumTransducerAsync<X, Y, A, B> m, 
        Func<B, SumTransducerAsync<X, Y, A, C>> g,
        Func<B, C, D> project) =>
        m.Bind(x => g(x).Map(y => project(x, y)));      
    
    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static SumTransducerAsync<X, Y, A, C> Apply<X, Y, A, B, C>(
        this SumTransducerAsync<X, Y, A, Func<B, C>> ff,
        SumTransducerAsync<X, Y, A, B> fa) =>
        new SumApplyTransducerAsync<X, Y, A, B, C>(ff, fa);
}
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
        Func<B, TransducerAsync<A, C>> g) =>
        new SumBindTransducerAsync3A<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, C> Bind<X, Y, A, B, C>(
        this TransducerAsync<A, B> m, 
        Func<B, SumTransducerAsync<X, Y, A, C>> g) =>
        new SumBindTransducerAsync3B<X, Y, A, B, C>(m, g);

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
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, D> SelectMany<X, Y, A, B, C, D>(
        this SumTransducerAsync<X, Y, A, B> m, 
        Func<B, TransducerAsync<A, C>> g,
        Func<B, C, D> project) =>
        m.Bind(x => g(x).Map(y => project(x, y)));      
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducerAsync<X, Y, A, D> SelectMany<X, Y, A, B, C, D>(
        this TransducerAsync<A, B> m, 
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
        
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<X, Y, A, B> Flatten<X, Y, A, B>(
        this SumTransducerAsync<X, Y, A, SumTransducerAsync<X, Y, A, B>> ff) =>
        new FlattenSumTransducerAsync1<X, Y, A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<X, Y, A, B> Flatten<X, Y, A, B>(
        this SumTransducerAsync<X, Y, A, SumTransducerAsync<X, Y, Unit, B>> ff) =>
        new FlattenSumTransducerAsync2<X, Y, A, B>(ff);
    
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<X, Y, A, B> Flatten<X, Y, A, B>(
        this SumTransducerAsync<X, Y, A, SumTransducer<X, Y, A, B>> ff) =>
        new FlattenSumTransducerAsyncSync1<X, Y, A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<X, Y, A, B> Flatten<X, Y, A, B>(
        this SumTransducerAsync<X, Y, A, SumTransducer<X, Y, Unit, B>> ff) =>
        new FlattenSumTransducerAsyncSync2<X, Y, A, B>(ff);
        
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<X, Y, A, B> Flatten<X, Y, A, B>(
        this SumTransducerAsync<X, Y, A, SumTransducerAsync<Unit, Unit, Unit, B>> ff) =>
        new FlattenSumTransducerAsync3<X, Y, A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<X, Y, A, B> Flatten<X, Y, A, B>(
        this SumTransducerAsync<X, Y, A, SumTransducer<Unit, Unit, Unit, B>> ff) =>
        new FlattenSumTransducerAsyncSync3<X, Y, A, B>(ff);
            
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<Env, X, Env, B> Flatten<Env, X, B>(
        this SumTransducerAsync<Env, X, Env, SumTransducer<Unit, X, Unit, B>> ff) =>
        new FlattenSumTransducerAsyncSync4<Env, X, Env, B>(ff);
    
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducerAsync<Env, X, Env, B> Flatten<Env, X, B>(
        this SumTransducerAsync<Env, X, Env, SumTransducerAsync<Unit, X, Unit, B>> ff) =>
        new FlattenSumTransducerAsync4<Env, X, Env, B>(ff);

}
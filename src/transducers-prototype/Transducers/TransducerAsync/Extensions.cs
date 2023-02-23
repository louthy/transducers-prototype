namespace LanguageExt;

public static partial class TransducerAsync
{
    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static TransducerAsync<A, C> Map<A, B, C>(this TransducerAsync<A, B> m, Func<B, C> g) =>
        new SelectTransducerAsync<A, B, C>(m, g);

    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static TransducerAsync<A, C> Select<A, B, C>(this TransducerAsync<A, B> m, Func<B, C> g) =>
        new SelectTransducerAsync<A, B, C>(m, g);
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static TransducerAsync<A, C> Bind<A, B, C>(this TransducerAsync<A, B> m, Func<B, TransducerAsync<A, C>> g) =>
        new BindTransducerAsync3<A, B, C>(m, g);
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static TransducerAsync<A, C> Bind<A, B, C>(this TransducerAsync<A, B> m, TransducerAsync<B, TransducerAsync<A, C>> g) =>
        new BindTransducerAsync1<A, B, C>(m, g);
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static TransducerAsync<A, C> Bind<A, B, C>(this TransducerAsync<A, B> m, TransducerAsync<B, Transducer<A, C>> g) =>
        new BindTransducerAsyncSync1<A, B, C>(m, g);
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static TransducerAsync<A, C> Bind<A, B, C>(this TransducerAsync<A, B> m, Func<B, Transducer<A, C>> g) =>
        new BindTransducerAsyncSync3<A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static TransducerAsync<A, C> SelectMany<A, B, C>(this TransducerAsync<A, B> m, Func<B, TransducerAsync<A, C>> g) =>
        new BindTransducerAsync3<A, B, C>(m, g);
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static TransducerAsync<A, D> SelectMany<A, B, C, D>(
        this TransducerAsync<A, B> m,
        Func<B, TransducerAsync<A, C>> g,
        Func<B, C, D> h) =>
        new SelectManyTransducerAsync2<A, B, C, D>(m, g, h);
    
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static TransducerAsync<A, B> Flatten<A, B>(this TransducerAsync<A, TransducerAsync<A, B>> ff) =>
        new FlattenTransducerAsync1<A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static TransducerAsync<A, B> Flatten<A, B>(this TransducerAsync<A, TransducerAsync<Unit, B>> ff) =>
        new FlattenTransducerAsync2<A, B>(ff);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static TransducerAsync<A, C> Apply<A, B, C>(
        this TransducerAsync<A, Func<B, C>> ff,
        TransducerAsync<A, B> fa) =>
        new ApplyTransducerAsync<A, B, C>(ff, fa);
        
    /// <summary>
    /// Lifts a unit accepting transducer, ignores the input value.
    /// </summary>
    public static TransducerAsync<A, B> Ignore<A, B>(this TransducerAsync<Unit, B> m) =>
        new IgnoreTransducerAsync<A, B>(m);
}
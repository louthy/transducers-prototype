namespace LanguageExt;

public static partial class Transducer
{
    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static Transducer<A, C> Map<A, B, C>(this Transducer<A, B> m, Func<B, C> g) =>
        new SelectTransducer<A, B, C>(m, g);

    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static Transducer<A, C> Select<A, B, C>(this Transducer<A, B> m, Func<B, C> g) =>
        new SelectTransducer<A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static Transducer<A, C> Bind<A, B, C>(this Transducer<A, B> m, Func<B, Transducer<A, C>> g) =>
        new BindTransducer3<A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static Transducer<A, C> Bind<A, B, C>(this Transducer<A, B> m, Transducer<B, Transducer<A, C>> g) =>
        new BindTransducer1<A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static Transducer<A, C> SelectMany<A, B, C>(this Transducer<A, B> m, Func<B, Transducer<A, C>> g) =>
        new BindTransducer3<A, B, C>(m, g);
    
    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static Transducer<A, D> SelectMany<A, B, C, D>(
        this Transducer<A, B> m, 
        Func<B, Transducer<A, C>> g,
        Func<B, C, D> h) =>
        new SelectManyTransducer2<A, B, C, D>(m, g, h);    
    
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static Transducer<A, B> Flatten<A, B>(this Transducer<A, Transducer<A, B>> ff) =>
        new FlattenTransducer1<A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static Transducer<A, B> Flatten<A, B>(this Transducer<A, Transducer<Unit, B>> ff) =>
        new FlattenTransducer2<A, B>(ff);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static Transducer<A, C> Apply<A, B, C>(
        this Transducer<A, Func<B, C>> ff,
        Transducer<A, B> fa) =>
        new ApplyTransducer<A, B, C>(ff, fa);
    
    /// <summary>
    /// Lifts a unit accepting transducer, ignores the input value.
    /// </summary>
    public static Transducer<A, B> Ignore<A, B>(this Transducer<Unit, B> m) =>
        new IgnoreTransducer<A, B>(m);
}
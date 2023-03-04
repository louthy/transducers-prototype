using LanguageExt.HKT;

namespace LanguageExt;

public static partial class Transducer
{
    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static SumTransducer<X, Y, A, C> Map<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, B> m, 
        Func<B, C> f) =>
        SumTransducer.compose(m, SumTransducer.mapRight<Y, B, C>(lift(f)));

    /// <summary>
    /// Maps every value passing through this transducer
    /// </summary>
    public static SumTransducer<X, Y, A, C> Select<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, B> m, 
        Func<B, C> f) =>
        SumTransducer.compose(m, SumTransducer.mapRight<Y, B, C>(lift(f)));

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducer<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, B> m, 
        Func<B, SumTransducer<X, Y, A, C>> g) =>
        new SumBindTransducer3<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducer<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, B> m, 
        Transducer<B, SumTransducer<X, Y, A, C>> g) =>
        new SumBindTransducer1<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducer<X, Y, A, C> Bind<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, B> m, 
        Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> g) =>
        new SumBindTransducer2<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducer<X, Y, A, C> SelectMany<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, B> m, 
        Func<B, SumTransducer<X, Y, A, C>> g) =>
        new SumBindTransducer3<X, Y, A, B, C>(m, g);

    /// <summary>
    /// Projects every value into the monadic bind function provided. 
    /// </summary>
    /// <returns>Monadic bound transducer</returns>
    public static SumTransducer<X, Y, A, D> SelectMany<X, Y, A, B, C, D>(
        this SumTransducer<X, Y, A, B> m, 
        Func<B, SumTransducer<X, Y, A, C>> g,
        Func<B, C, D> project) =>
        m.Bind(x => g(x).Map(y => project(x, y)));    
    
    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static SumTransducer<X, Y, A, C> Apply<X, Y, A, B, C>(
        this SumTransducer<X, Y, A, Func<B, C>> ff,
        SumTransducer<X, Y, A, B> fa) =>
        new SumApplyTransducer<X, Y, A, B, C>(ff, fa);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducer<X, Y, A, B> Flatten<X, Y, A, B>(this SumTransducer<X, Y, A, SumTransducer<X, Y, A, B>> ff) =>
        new FlattenSumTransducer1<X, Y, A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducer<X, Y, A, B> Flatten<X, Y, A, B>(this SumTransducer<X, Y, A, SumTransducer<X, Y, Unit, B>> ff) =>
        new FlattenSumTransducer2<X, Y, A, B>(ff);
    
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducer<X, Y, A, B> Flatten<X, Y, A, B>(this SumTransducer<X, Y, A, SumTransducer<Unit, Unit, Unit, B>> ff) =>
        new FlattenSumTransducer3<X, Y, A, B>(ff);
    
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static SumTransducer<Env, X, Env, B> Flatten<Env, X, B>(this SumTransducer<Env, X, Env, SumTransducer<Unit, X, Unit, B>> ff) =>
        new FlattenSumTransducer4<Env, X, Env, B>(ff);

}
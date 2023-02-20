#nullable enable
using System;

namespace LanguageExt;

public static partial class Transducer
{
    /// <summary>
    /// Lift a value into the `Transducer` space
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <typeparam name="A">Value type</typeparam>
    /// <returns>`Transducer` from `Unit` to `A`</returns>
    public static Transducer<Unit, A> Pure<A>(A value) =>
        constant<Unit, A>(value);

    /// <summary>
    /// Identity transducer
    /// </summary>
    public static Transducer<A, A> identity<A>() =>
        IdentityTransducer<A>.Default;

    /// <summary>
    /// Constant transducer
    /// </summary>
    /// <remarks>
    /// Takes any value, ignores it and yields the value provided.
    /// </remarks>
    /// <param name="value">Constant value to yield</param>
    /// <typeparam name="A">Input value type</typeparam>
    /// <typeparam name="B">Constant value type</typeparam>
    /// <returns>`Transducer` from `A` to `B`</returns>
    public static Transducer<A, B> constant<A, B>(B value) =>
        new ConstantTransducer<A, B>(value);
    
    public static Transducer<Unit, A> lift<A>(Func<A> f) =>
        new LiftTransducer4<A>(f);
    
    public static Transducer<A, B> lift<A, B>(Func<A, B> f) =>
        new LiftTransducer3<A, B>(f);
    
    public static Transducer<A, B> lift<A, B>(Func<A, TResult<B>> f) =>
        new LiftTransducer1<A, B>(f);
    
    public static Transducer<Unit, A> lift<A>(Func<TResult<A>> f) =>
        new LiftTransducer2<A>(f);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static Transducer<A, C> compose<A, B, C>(
        Transducer<A, B> f, 
        Transducer<B, C> g) =>
        new ComposeTransducer<A, B, C>(f, g);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static Transducer<A, D> compose<A, B, C, D>(
        Transducer<A, B> f, 
        Transducer<B, C> g, 
        Transducer<C, D> h) =>
        new ComposeTransducer<A, B, C, D>(f, g, h);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static Transducer<A, E> compose<A, B, C, D, E>(
        Transducer<A, B> f, 
        Transducer<B, C> g, 
        Transducer<C, D> h,
        Transducer<D, E> i) =>
        new ComposeTransducer<A, B, C, D, E>(f, g, h, i);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static Transducer<A, F> compose<A, B, C, D, E, F>(
        Transducer<A, B> f, 
        Transducer<B, C> g, 
        Transducer<C, D> h,
        Transducer<D, E> i,
        Transducer<E, F> j) =>
        new ComposeTransducer<A, B, C, D, E, F>(f, g, h, i, j);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static Transducer<A, G> compose<A, B, C, D, E, F, G>(
        Transducer<A, B> f, 
        Transducer<B, C> g, 
        Transducer<C, D> h,
        Transducer<D, E> i,
        Transducer<E, F> j,
        Transducer<F, G> k) =>
        new ComposeTransducer<A, B, C, D, E, F, G>(f, g, h, i, j, k);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static Transducer<A, B> flatten<A, B>(Transducer<A, Transducer<A, B>> ff) =>
        new FlattenTransducer1<A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static Transducer<A, B> flatten<A, B>(Transducer<A, Transducer<Unit, B>> ff) =>
        new FlattenTransducer2<A, B>(ff);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static Transducer<A, C> apply<A, B, C>(
        Transducer<A, Func<B, C>> ff,
        Transducer<A, B> fa) =>
        new ApplyTransducer<A, B, C>(ff, fa);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static Transducer<A, C> bind<A, B, C>(
        Transducer<A, B> m,
        Transducer<B, Transducer<A, C>> f) =>
        new BindTransducer1<A, B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static Transducer<A, C> bind<A, B, C>(
        Transducer<A, B> m,
        Transducer<B, Func<A, C>> f) =>
        new BindTransducer2<A, B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static Transducer<A, C> bind<A, B, C>(
        Transducer<A, B> m,
        Func<B, Transducer<A, C>> f) =>
        new BindTransducer3<A, B, C>(m, f);
}

#nullable enable
using System;

namespace LanguageExt;

// ---------------------------------------------------------------------------------------------------------------------

public static partial class TransducerAsync
{
    /// <summary>
    /// Lift a value into the `Transducer` space
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <typeparam name="A">Value type</typeparam>
    /// <returns>`Transducer` from `Unit` to `A`</returns>
    public static TransducerAsync<Unit, A> Pure<A>(A value) =>
        constant<Unit, A>(value);

    /// <summary>
    /// Identity transducer
    /// </summary>
    public static TransducerAsync<A, A> identity<A>() =>
        IdentityTransducerAsync<A>.Default;
    
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
    public static TransducerAsync<A, B> constant<A, B>(B value) =>
        new ConstantTransducerAsync<A, B>(value);
    
    public static TransducerAsync<A, B> lift<A, B>(Func<A, TResultAsync<B>> f) =>
        new LiftTransducerAsync1<A, B>(f);
    
    public static TransducerAsync<Unit, A> lift<A>(Func<TResultAsync<A>> f) =>
        new LiftTransducerAsync2<A>(f);
    
    public static TransducerAsync<A, B> lift<A, B>(Func<A, B> f) =>
        new LiftTransducerAsync3<A, B>(f);
    
    public static TransducerAsync<Unit, A> lift<A>(Func<A> f) =>
        new LiftTransducerAsync4<A>(f);
    
    public static TransducerAsync<A, B> lift<A, B>(Func<A, ValueTask<B>> f) =>
        new LiftTransducerAsync5<A, B>(f);
    
    public static TransducerAsync<Unit, A> lift<A>(Func<ValueTask<A>> f) =>
        new LiftTransducerAsync6<A>(f);
    
    public static TransducerAsync<A, B> lift2<A, B>(Func<A, Task<B>> f) =>
        new LiftTransducerAsync7<A, B>(f);
    
    public static TransducerAsync<Unit, A> lift2<A>(Func<Task<A>> f) =>
        new LiftTransducerAsync8<A>(f);
    
    public static TransducerAsync<A, B> lift<A, B>(Func<A, ValueTask<TResultAsync<B>>> f) =>
        new LiftTransducerAsync9<A, B>(f);
    
    public static TransducerAsync<Unit, A> lift<A>(Func<ValueTask<TResultAsync<A>>> f) =>
        new LiftTransducerAsync10<A>(f);
    
    public static TransducerAsync<A, B> lift2<A, B>(Func<A, Task<TResultAsync<B>>> f) =>
        new LiftTransducerAsync11<A, B>(f);
    
    public static TransducerAsync<Unit, A> lift2<A>(Func<Task<TResultAsync<A>>> f) =>
        new LiftTransducerAsync12<A>(f);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static TransducerAsync<A, C> compose<A, B, C>(TransducerAsync<A, B> f, TransducerAsync<B, C> g) =>
        new ComposeTransducerAsync<A, B, C>(f, g);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static TransducerAsync<A, D> compose<A, B, C, D>(TransducerAsync<A, B> f, TransducerAsync<B, C> g, TransducerAsync<C, D> h) =>
        new ComposeTransducerAsync<A, B, C, D>(f, g, h);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static TransducerAsync<A, E> compose<A, B, C, D, E>(
        TransducerAsync<A, B> f, 
        TransducerAsync<B, C> g, 
        TransducerAsync<C, D> h,
        TransducerAsync<D, E> i) =>
        new ComposeTransducerAsync<A, B, C, D, E>(f, g, h, i);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static TransducerAsync<A, F> compose<A, B, C, D, E, F>(
        TransducerAsync<A, B> f, 
        TransducerAsync<B, C> g, 
        TransducerAsync<C, D> h,
        TransducerAsync<D, E> i,
        TransducerAsync<E, F> j) =>
        new ComposeTransducerAsync<A, B, C, D, E, F>(f, g, h, i, j);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static TransducerAsync<A, G> compose<A, B, C, D, E, F, G>(
        TransducerAsync<A, B> f, 
        TransducerAsync<B, C> g, 
        TransducerAsync<C, D> h,
        TransducerAsync<D, E> i,
        TransducerAsync<E, F> j,
        TransducerAsync<F, G> k) =>
        new ComposeTransducerAsync<A, B, C, D, E, F, G>(f, g, h, i, j, k);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static TransducerAsync<A, B> flatten<A, B>(TransducerAsync<A, TransducerAsync<A, B>> ff) =>
        new FlattenTransducerAsync1<A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static TransducerAsync<A, B> flatten<A, B>(TransducerAsync<A, TransducerAsync<Unit, B>> ff) =>
        new FlattenTransducerAsync2<A, B>(ff);
    
    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static TransducerAsync<A, C> apply<A, B, C>(
        TransducerAsync<A, Func<B, C>> ff,
        TransducerAsync<A, B> fa) =>
        new ApplyTransducerAsync<A, B, C>(ff, fa);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static TransducerAsync<A, C> bind<A, B, C>(
        TransducerAsync<A, B> m,
        TransducerAsync<B, TransducerAsync<A, C>> f) =>
        new BindTransducerAsync1<A, B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static TransducerAsync<A, C> bind<A, B, C>(
        TransducerAsync<A, B> m,
        TransducerAsync<B, Transducer<A, C>> f) =>
        new BindTransducerAsyncSync1<A, B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static TransducerAsync<A, C> bind<A, B, C>(
        TransducerAsync<A, B> m,
        TransducerAsync<B, Func<A, C>> f) =>
        new BindTransducerAsync2<A, B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static TransducerAsync<A, C> bind<A, B, C>(
        TransducerAsync<A, B> m,
        Func<B, TransducerAsync<A, C>> f) =>
        new BindTransducerAsync3<A, B, C>(m, f);    

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static TransducerAsync<A, C> bind<A, B, C>(
        TransducerAsync<A, B> m,
        Func<B, Transducer<A, C>> f) =>
        new BindTransducerAsyncSync3<A, B, C>(m, f);    
    
    /// <summary>
    /// Lifts a unit accepting transducer, ignores the input value.
    /// </summary>
    public static TransducerAsync<A, B> ignore<A, B>(TransducerAsync<Unit, B> m) =>
        new IgnoreTransducerAsync<A, B>(m);
}

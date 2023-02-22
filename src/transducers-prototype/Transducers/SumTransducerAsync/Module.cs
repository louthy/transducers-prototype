namespace LanguageExt;

public static class SumTransducerAsync
{
    /// <summary>
    /// Make a sum-transducer from a pair of transducers
    /// </summary>
    /// <param name="left">Left transducer</param>
    /// <param name="right">Right transducer</param>
    /// <returns>Sum transducer</returns>
    public static SumTransducerAsync<X, Y, A, B> make<X, Y, A, B>(
        TransducerAsync<X, Sum<Y, B>> left, 
        TransducerAsync<A, Sum<Y, B>> right) =>
        new LiftSumTransducerAsync<X, Y, A, B>(left, right);

    /// <summary>
    /// Lift a sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> Pure<X, A>(Sum<X, A> value) =>
        constant<Unit, X, Unit, A>(value);

    /// <summary>
    /// Lift a constant into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Right` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> Right<X, A>(A value) =>
        Pure(Sum<X, A>.Right(value));

    /// <summary>
    /// Lift a constant into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Left` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> Left<X, A>(X value) =>
        Pure(Sum<X, A>.Left(value));
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducerAsync<T, V, A, C> compose<T, U, V, A, B, C>(
        SumTransducerAsync<T, U, A, B> f, 
        SumTransducerAsync<U, V, B, C> g) =>
        new ComposeSumTransducerAsync<T, U, V, A, B, C>(f, g);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducerAsync<T, W, A, D> compose<T, U, V, W, A, B, C, D>(
        SumTransducerAsync<T, U, A, B> f, 
        SumTransducerAsync<U, V, B, C> g, 
        SumTransducerAsync<V, W, C, D> h) =>
        new ComposeSumTransducerAsync<T, U, V, W, A, B, C, D>(f, g, h);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducerAsync<T, X, A, E> compose<T, U, V, W, X, A, B, C, D, E>(
        SumTransducerAsync<T, U, A, B> f, 
        SumTransducerAsync<U, V, B, C> g, 
        SumTransducerAsync<V, W, C, D> h,
        SumTransducerAsync<W, X, D, E> i) =>
        new ComposeSumTransducerAsync<T, U, V, W, X, A, B, C, D, E>(f, g, h, i);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducerAsync<T, Y, A, F> compose<T, U, V, W, X, Y, A, B, C, D, E, F>(
        SumTransducerAsync<T, U, A, B> f, 
        SumTransducerAsync<U, V, B, C> g, 
        SumTransducerAsync<V, W, C, D> h,
        SumTransducerAsync<W, X, D, E> i,
        SumTransducerAsync<X, Y, E, F> j) =>
        new ComposeSumTransducerAsync<T, U, V, W, X, Y, A, B, C, D, E, F>(f, g, h, i, j);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducerAsync<T, Z, A, G> compose<T, U, V, W, X, Y, Z, A, B, C, D, E, F, G>(
        SumTransducerAsync<T, U, A, B> f, 
        SumTransducerAsync<U, V, B, C> g, 
        SumTransducerAsync<V, W, C, D> h,
        SumTransducerAsync<W, X, D, E> i,
        SumTransducerAsync<X, Y, E, F> j,
        SumTransducerAsync<Y, Z, F, G> k) =>
        new ComposeSumTransducerAsync<T, U, V, W, X, Y, Z, A, B, C, D, E, F, G>(f, g, h, i, j, k);
    
    /// <summary>
    /// Identity transducer
    /// </summary>
    public static SumTransducerAsync<X, X, A, A> identity<X, A>() =>
        make(TransducerAsync.lift<X, Sum<X, A>>(x => Sum<X, A>.Left(x)),
             TransducerAsync.lift<A, Sum<X, A>>(a => Sum<X, A>.Right(a)));
    
    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <remarks>This transducer effectively ignores the input value and yields the provided constant sum
    /// value.</remarks>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducerAsync<X, Y, A, B> constant<X, Y, A, B>(Sum<Y, B> value) =>
        make(TransducerAsync.constant<X, Sum<Y, B>>(value), TransducerAsync.constant<A, Sum<Y, B>>(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> lift<X, A>(Func<Sum<X, A>> value) =>
        make(TransducerAsync.lift(value), 
             TransducerAsync.lift(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> lift<X, A>(Func<TResultAsync<Sum<X, A>>> value) =>
        make(TransducerAsync.lift(value), 
             TransducerAsync.lift(value));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducerAsync<X, Y, A, B> lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> value) =>
        make(TransducerAsync.lift((X x) => value(Sum<X, A>.Left(x))),
             TransducerAsync.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducerAsync<X, Y, A, B> lift<X, Y, A, B>(
        Func<Sum<X, A>, TResultAsync<Sum<Y, B>>> value) =>
        make(TransducerAsync.lift((X x) => value(Sum<X, A>.Left(x))),
             TransducerAsync.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> lift<X, A>(
        Func<ValueTask<Sum<X, A>>> value) =>
        make(TransducerAsync.lift(value), 
             TransducerAsync.lift(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducerAsync<Unit, X, Unit, A> lift<X, A>(
        Func<ValueTask<TResultAsync<Sum<X, A>>>> value) =>
        make(TransducerAsync.lift(value), 
             TransducerAsync.lift(value));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducerAsync<X, Y, A, B> lift<X, Y, A, B>(Func<Sum<X, A>, ValueTask<Sum<Y, B>>> value) =>
        make(TransducerAsync.lift((X x) => value(Sum<X, A>.Left(x))),
             TransducerAsync.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Bi-mapping transducers
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static SumTransducerAsync<X, Y, A, B> bimap<X, Y, A, B>(
        TransducerAsync<X, Y> Left,
        TransducerAsync<A, B> Right) =>
        make(Left.Map(Sum<Y, B>.Left), Right.Map(Sum<Y, B>.Right));

    /// <summary>
    /// Map left transducer
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static SumTransducerAsync<X, Y, A, A> mapLeft<X, Y, A>(TransducerAsync<X, Y> Left) =>
        bimap(Left, TransducerAsync.identity<A>());

    /// <summary>
    /// Map right transducer
    /// </summary>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static SumTransducerAsync<X, X, A, B> mapRight<X, A, B>(TransducerAsync<A, B> Right) =>
        bimap(TransducerAsync.identity<X>(), Right);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static SumTransducerAsync<X, Y, A, C> apply<X, Y, A, B, C>(
        SumTransducerAsync<X, Y, A, Func<B, C>> ff,
        SumTransducerAsync<X, Y, A, B> fa) =>
        new SumApplyTransducerAsync<X, Y, A, B, C>(ff, fa);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducerAsync<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducerAsync<X, Y, A, B> m,
        Func<B, SumTransducerAsync<X, Y, A, C>> f) =>
        new SumBindTransducerAsync3<X, Y, A, B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducerAsync<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducerAsync<X, Y, A, B> m,
        TransducerAsync<B, Func<Sum<X, A>, Sum<Y, C>>> f) =>
        new SumBindTransducerAsync2<X, Y, A , B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducerAsync<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducerAsync<X, Y, A, B> m,
        TransducerAsync<B, SumTransducerAsync<X, Y, A, C>> f) =>
        new SumBindTransducerAsync1<X, Y, A , B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducerAsync<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducerAsync<X, Y, A, B> m,
        TransducerAsync<B, SumTransducer<X, Y, A, C>> f) =>
        new SumBindTransducerAsyncSync1<X, Y, A , B, C>(m, f);    
}
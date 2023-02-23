namespace LanguageExt;

public static class SumTransducer
{
    /// <summary>
    /// Make a sum-transducer from a pair of transducers
    /// </summary>
    /// <param name="left">Left transducer</param>
    /// <param name="right">Right transducer</param>
    /// <returns>Sum transducer</returns>
    public static SumTransducer<X, Y, A, B> make<X, Y, A, B>(
        Transducer<X, Sum<Y, B>> left, 
        Transducer<A, Sum<Y, B>> right) =>
        new LiftSumTransducer<X, Y, A, B>(left, right);

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducer<Unit, X, Unit, A> Pure<X, A>(Sum<X, A> value) =>
        constant<Unit, X, Unit, A>(value);

    /// <summary>
    /// Lift a constant into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Right` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducer<Unit, X, Unit, A> Right<X, A>(A value) =>
        Pure(Sum<X, A>.Right(value));

    /// <summary>
    /// Lift a constant into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Left` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducer<Unit, X, Unit, A> Left<X, A>(X value) =>
        Pure(Sum<X, A>.Left(value));
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducer<T, V, A, C> compose<T, U, V, A, B, C>(
        SumTransducer<T, U, A, B> f, 
        SumTransducer<U, V, B, C> g) =>
        new ComposeSumTransducer<T, U, V, A, B, C>(f, g);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducer<T, W, A, D> compose<T, U, V, W, A, B, C, D>(
        SumTransducer<T, U, A, B> f, 
        SumTransducer<U, V, B, C> g, 
        SumTransducer<V, W, C, D> h) =>
        new ComposeSumTransducer<T, U, V, W, A, B, C, D>(f, g, h);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducer<T, X, A, E> compose<T, U, V, W, X, A, B, C, D, E>(
        SumTransducer<T, U, A, B> f, 
        SumTransducer<U, V, B, C> g, 
        SumTransducer<V, W, C, D> h,
        SumTransducer<W, X, D, E> i) =>
        new ComposeSumTransducer<T, U, V, W, X, A, B, C, D, E>(f, g, h, i);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducer<T, Y, A, F> compose<T, U, V, W, X, Y, A, B, C, D, E, F>(
        SumTransducer<T, U, A, B> f, 
        SumTransducer<U, V, B, C> g, 
        SumTransducer<V, W, C, D> h,
        SumTransducer<W, X, D, E> i,
        SumTransducer<X, Y, E, F> j) =>
        new ComposeSumTransducer<T, U, V, W, X, Y, A, B, C, D, E, F>(f, g, h, i, j);
    
    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public static SumTransducer<T, Z, A, G> compose<T, U, V, W, X, Y, Z, A, B, C, D, E, F, G>(
        SumTransducer<T, U, A, B> f, 
        SumTransducer<U, V, B, C> g, 
        SumTransducer<V, W, C, D> h,
        SumTransducer<W, X, D, E> i,
        SumTransducer<X, Y, E, F> j,
        SumTransducer<Y, Z, F, G> k) =>
        new ComposeSumTransducer<T, U, V, W, X, Y, Z, A, B, C, D, E, F, G>(f, g, h, i, j, k);
    
    /// <summary>
    /// Identity transducer
    /// </summary>
    public static SumTransducer<X, X, A, A> identity<X, A>() =>
        make(Transducer.lift<X, Sum<X, A>>(x => Sum<X, A>.Left(x)),
             Transducer.lift<A, Sum<X, A>>(a => Sum<X, A>.Right(a)));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <remarks>This transducer effectively ignores the input value and yields the provided constant sum
    /// value.</remarks>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducer<X, Y, A, B> constant<X, Y, A, B>(Sum<Y, B> value) =>
        make(Transducer.constant<X, Sum<Y, B>>(value), 
             Transducer.constant<A, Sum<Y, B>>(value));

    /// <summary>
    /// Lift into the sum-transducer
    /// </summary>
    public static SumTransducer<X, Y, A, B> lift<X, Y, A, B>(Transducer<Sum<X, A>, Sum<Y, B>> f) =>
        new LiftSumTransducer2<X, Y, A, B>(f);

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducer<Unit, X, Unit, A> lift<X, A>(Func<Sum<X, A>> value) =>
        make(Transducer.lift(value), 
             Transducer.lift(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static SumTransducer<Unit, X, Unit, A> lift<X, A>(Func<TResult<Sum<X, A>>> value) =>
        make(Transducer.lift(value), 
             Transducer.lift(value));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducer<X, Y, A, B> lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> value) =>
        make(Transducer.lift((X x) => value(Sum<X, A>.Left(x))),
             Transducer.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static SumTransducer<X, Y, A, B> lift<X, Y, A, B>(Func<Sum<X, A>, TResult<Sum<Y, B>>> value) =>
        make(Transducer.lift((X x) => value(Sum<X, A>.Left(x))),
             Transducer.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Bi-mapping transducers
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static SumTransducer<X, Y, A, B> bimap<X, Y, A, B>(
        Transducer<X, Y> Left,
        Transducer<A, B> Right) =>
        make(Left.Map(Sum<Y, B>.Left), Right.Map(Sum<Y, B>.Right));

    /// <summary>
    /// Map left transducer
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static SumTransducer<X, Y, A, A> mapLeft<X, Y, A>(Transducer<X, Y> Left) =>
        bimap(Left, Transducer.identity<A>());

    /// <summary>
    /// Map right transducer
    /// </summary>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static SumTransducer<X, X, A, B> mapRight<X, A, B>(Transducer<A, B> Right) =>
        bimap(Transducer.identity<X>(), Right);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static SumTransducer<X, Y, A, C> apply<X, Y, A, B, C>(
        SumTransducer<X, Y, A, Func<B, C>> ff,
        SumTransducer<X, Y, A, B> fa) =>
        new SumApplyTransducer<X, Y, A, B, C>(ff, fa);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducer<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducer<X, Y, A, B> m,
        Func<B, SumTransducer<X, Y, A, C>> f) =>
        new SumBindTransducer3<X, Y, A , B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducer<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducer<X, Y, A, B> m,
        Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> f) =>
        new SumBindTransducer2<X, Y, A , B, C>(m, f);

    /// <summary>
    /// Monadic bind
    /// </summary>
    public static SumTransducer<X, Y, A, C> bind<X, Y, A, B, C>(
        SumTransducer<X, Y, A, B> m,
        Transducer<B, SumTransducer<X, Y, A, C>> f) =>
        new SumBindTransducer1<X, Y, A , B, C>(m, f);
    
    public static SumTransducer<X, Y, A, B> ignore<X, Y, A, B>(SumTransducer<Unit, Unit, Unit, B> m) =>
        Transducer.ignore<Sum<X, A>, Sum<Y, B>>(m)
}
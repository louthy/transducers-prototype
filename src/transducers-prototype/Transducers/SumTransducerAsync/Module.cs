namespace LanguageExt;

public static class SumTransducerAsync
{
    /// <summary>
    /// Make a sum-transducer from a pair of transducers
    /// </summary>
    /// <param name="left">Left transducer</param>
    /// <param name="right">Right transducer</param>
    /// <returns>Sum transducer</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, B>> make<X, Y, A, B>(
        TransducerAsync<X, Sum<Y, B>> left, 
        TransducerAsync<A, Sum<Y, B>> right) =>
        new SumTransducerAsync<X, Y, A, B>(left, right);

    /// <summary>
    /// Lift a sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit` sum is applied</returns>
    public static TransducerAsync<Sum<Unit, Unit>, Sum<X, A>> Pure<X, A>(Sum<X, A> value) =>
        constant<Unit, X, Unit, A>(value);

    /// <summary>
    /// Identity transducer
    /// </summary>
    public static TransducerAsync<Sum<X, A>, Sum<X, A>> identity<X, A>() =>
        make(TransducerAsync.lift<X, Sum<X, A>>(x => Sum<X, A>.Left(x)),
             TransducerAsync.lift<A, Sum<X, A>>(a => Sum<X, A>.Right(a)));
    
    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <remarks>This transducer effectively ignores the input value and yields the provided constant sum
    /// value.</remarks>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, B>> constant<X, Y, A, B>(Sum<Y, B> value) =>
        make(TransducerAsync.constant<X, Sum<Y, B>>(value), TransducerAsync.constant<A, Sum<Y, B>>(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static TransducerAsync<Sum<Unit, Unit>, Sum<X, A>> lift<X, A>(Func<Sum<X, A>> value) =>
        make(TransducerAsync.lift(value), 
             TransducerAsync.lift(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static TransducerAsync<Sum<Unit, Unit>, Sum<X, A>> lift<X, A>(Func<TResultAsync<Sum<X, A>>> value) =>
        make(TransducerAsync.lift(value), 
             TransducerAsync.lift(value));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, B>> lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> value) =>
        make(TransducerAsync.lift((X x) => value(Sum<X, A>.Left(x))),
             TransducerAsync.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, B>> lift<X, Y, A, B>(
        Func<Sum<X, A>, TResultAsync<Sum<Y, B>>> value) =>
        make(TransducerAsync.lift((X x) => value(Sum<X, A>.Left(x))),
             TransducerAsync.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static TransducerAsync<Sum<Unit, Unit>, Sum<X, A>> lift<X, A>(
        Func<ValueTask<Sum<X, A>>> value) =>
        make(TransducerAsync.lift(value), 
            TransducerAsync.lift(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static TransducerAsync<Sum<Unit, Unit>, Sum<X, A>> lift<X, A>(
        Func<ValueTask<TResultAsync<Sum<X, A>>>> value) =>
        make(TransducerAsync.lift(value), 
            TransducerAsync.lift(value));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, B>> lift<X, Y, A, B>(Func<Sum<X, A>, ValueTask<Sum<Y, B>>> value) =>
        make(TransducerAsync.lift((X x) => value(Sum<X, A>.Left(x))),
             TransducerAsync.lift((A a) => value(Sum<X, A>.Right(a))));
    
    /// <summary>
    /// Bi-mapping transducers
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, B>> bimap<X, Y, A, B>(
        TransducerAsync<X, Y> Left,
        TransducerAsync<A, B> Right) =>
        make(Left.Map(Sum<Y, B>.Left), Right.Map(Sum<Y, B>.Right));

    /// <summary>
    /// Map left transducer
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, A>> mapLeft<X, Y, A>(TransducerAsync<X, Y> Left) =>
        bimap(Left, TransducerAsync.identity<A>());

    /// <summary>
    /// Map right transducer
    /// </summary>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static TransducerAsync<Sum<X, A>, Sum<X, B>> mapRight<X, A, B>(TransducerAsync<A, B> Right) =>
        bimap(TransducerAsync.identity<X>(), Right);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, C>> apply<X, Y, A, B, C>(
        TransducerAsync<Sum<X, A>, Sum<Y, Func<B, C>>> ff,
        TransducerAsync<Sum<X, A>, Sum<Y, B>> fa) =>
        new SumApplyTransducerAsync<X, Y, A, B, C>(ff, fa);
}
namespace LanguageExt;

/// <summary>
/// Sum-transducers are a pair of `Transducer` values that work on `Sum` values  (i.e. discriminated unions).
/// The transducer used depends on whether the `Sum` value is in a `Left` or `Right` state.  
/// </summary>
/// <summary>
/// Transducers are composable algorithmic transformations. They are independent from the context of their input and
/// output sources and specify only the essence of the transformation in terms of an individual element. Because
/// transducers are decoupled from input or output sources, they can be used in many different processes -
/// collections, streams, channels, observables, etc. Transducers compose directly, without awareness of input or
/// creation of intermediate aggregates.
/// </summary>
/// <typeparam name="X">Left input value type</typeparam>
/// <typeparam name="Y">Right output value type</typeparam>
/// <typeparam name="A">Left input value type</typeparam>
/// <typeparam name="B">Right output value type</typeparam>
public record SumTransducerAsync<X, Y, A, B>(TransducerAsync<X, Sum<Y, B>> Left, TransducerAsync<A, Sum<Y, B>> Right) 
    : TransducerAsync<Sum<X, A>, Sum<Y, B>>
{
    /// <summary>
    /// Transform the transducer using the reducer provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Reducer that captures the transformation of the `Transducer` and the provided reducer</returns>
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(Left, Right, reduce);

    internal record Reduce<S>(
        TransducerAsync<X, Sum<Y, B>> Left, 
        TransducerAsync<A, Sum<Y, B>> Right, 
        ReducerAsync<S, Sum<Y, B>> Reducer) : 
        ReducerAsync<S,Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> v) =>
            v switch
            {
                SumRight<X, A> r => Right.Transform(Reducer).Run(st, s, r.Value),
                SumLeft<X, A> l => Left.Transform(Reducer).Run(st, s, l.Value),
                _ => new(TResultAsync.Complete(s))
            };
    }
}
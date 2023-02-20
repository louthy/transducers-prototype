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
record SumTransducer<X, Y, A, B>(Transducer<X, Sum<Y, B>> Left, Transducer<A, Sum<Y, B>> Right) 
    : Transducer<Sum<X, A>, Sum<Y, B>>
{
    /// <summary>
    /// Transform the transducer using the reducer provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Reducer that captures the transformation of the `Transducer` and the provided reducer</returns>
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(Left, Right, reduce);
    
    /// <summary>
    /// Lift the synchronous sum-transducer into the asynchronous space 
    /// </summary>
    public override TransducerAsync<Sum<X, A>, Sum<Y, B>> ToAsync() =>
        new SumTransducerAsync<X, Y, A, B>(Left.ToAsync(), Right.ToAsync());
    
    record Reduce<S>(Transducer<X, Sum<Y, B>> Left, Transducer<A, Sum<Y, B>> Right, Reducer<S, Sum<Y, B>> Reducer) : 
        Reducer<S,Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> v) =>
            v switch
            {
                SumRight<X, A> r => Right.Transform(Reducer).Run(st, s, r.Value),
                SumLeft<X, A> l => Left.Transform(Reducer).Run(st, s, l.Value),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new SumTransducerAsync<X, Y, A, B>.Reduce<S>(Left.ToAsync(), Right.ToAsync(), Reducer.ToAsync());
    }
}
namespace LanguageExt;

/// <summary>
/// Ignore transducer.  Lifts a unit accepting transducer, ignores the input value.
/// </summary>
record IgnoreSumTransducer<X, Y, A, B>(SumTransducer<Unit, Y, Unit, B> Transducer) : SumTransducer<X, Y, A, B>
{
    static readonly Sum<Unit, Unit> runit = Sum<Unit, Unit>.Right(default);
    static readonly Sum<Unit, Unit> lunit = Sum<Unit, Unit>.Left(default);
        
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Ignore<S>(Transducer.Transform(reduce));

    public override TransducerAsync<Sum<X, A>, Sum<Y, B>> ToAsync() =>
        ToSumAsync(); 

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        new IgnoreSumTransducerAsync<X, Y, A, B>(Transducer.ToSumAsync());

    record Ignore<S>(Reducer<S, Sum<Unit, Unit>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState state, S stateValue, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> => Reducer.Run(state, stateValue, runit),
                SumLeft<X, A> => Reducer.Run(state, stateValue, lunit),
                _ => TResult.Complete(stateValue)
            };

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new IgnoreSumTransducerAsync<X, Y, A, B>.Ignore<S>(Reducer.ToAsync());
    }
}
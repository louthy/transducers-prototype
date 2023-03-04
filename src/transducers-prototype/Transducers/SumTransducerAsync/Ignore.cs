namespace LanguageExt;

/// <summary>
/// Ignore transducer.  Lifts a unit accepting transducer, ignores the input value.
/// </summary>
record IgnoreSumTransducerAsync<X, Y, A, B>(SumTransducerAsync<Unit, Y, Unit, B> Transducer) : SumTransducerAsync<X, Y, A, B>
{
    static readonly Sum<Unit, Unit> runit = Sum<Unit, Unit>.Right(default);
    static readonly Sum<Unit, Unit> lunit = Sum<Unit, Unit>.Left(default);
        
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Ignore<S>(Transducer.Transform(reduce));

    internal record Ignore<S>(ReducerAsync<S, Sum<Unit, Unit>> Reducer) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> => Reducer.Run(state, stateValue, runit),
                SumLeft<X, A> => Reducer.Run(state, stateValue, lunit),
                _ => TResultAsync.Complete(stateValue)
            };
    }
}
namespace LanguageExt;

/// <summary>
/// Ignore transducer.  Lifts a unit accepting transducer, ignores the input value.
/// </summary>
record IgnoreTransducer<A, B>(Transducer<Unit, B> Transducer) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Ignore<S>(Transducer.Transform(reduce));

    public override TransducerAsync<A, B> ToAsync() =>
        new IgnoreTransducerAsync<A, B>(Transducer.ToAsync());
    
    record Ignore<S>(Reducer<S, Unit> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState state, S stateValue, A value) =>
            Reducer.Run(state, stateValue, default);

        public override ReducerAsync<S, A> ToAsync() =>
            new IgnoreTransducerAsync<A, B>.Ignore<S>(Reducer.ToAsync());
    }
}
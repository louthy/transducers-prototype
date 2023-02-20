namespace LanguageExt;

/// <summary>
/// Identity transducer, simply passes the value through 
/// </summary>
record IdentityTransducer<A> : Transducer<A, A>
{
    public static Transducer<A, A> Default = new IdentityTransducer<A>();

    public override Reducer<S, A> Transform<S>(Reducer<S, A> reduce) =>
        new Reduce<S>(reduce);

    public override TransducerAsync<A, A> ToAsync() =>
        IdentityTransducerAsync<A>.Default;

    record Reduce<S>(Reducer<S, A> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState state, S stateValue, A value) =>
            Reducer.Run(state, stateValue, value);

        public override ReducerAsync<S, A> ToAsync() =>
            new IdentityTransducerAsync<A>.Reduce<S>(Reducer.ToAsync());
    }
}
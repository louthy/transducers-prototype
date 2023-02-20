namespace LanguageExt;

/// <summary>
/// Identity transducer, simply passes the value through 
/// </summary>
record IdentityTransducerAsync<A> : TransducerAsync<A, A>
{
    public static TransducerAsync<A, A> Default = new IdentityTransducerAsync<A>();

    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(reduce);

    internal record Reduce<S>(ReducerAsync<S, A> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, A value) =>
            Reducer.Run(state, stateValue, value);
    }
}
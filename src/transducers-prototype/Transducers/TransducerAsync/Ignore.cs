namespace LanguageExt;

/// <summary>
/// Ignore transducerAsync.  Lifts a unit accepting transducerAsync, ignores the input value.
/// </summary>
record IgnoreTransducerAsync<A, B>(TransducerAsync<Unit, B> TransducerAsync) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Ignore<S>(TransducerAsync.Transform(reduce));

    internal record Ignore<S>(ReducerAsync<S, Unit> ReducerAsync) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, A value) =>
            ReducerAsync.Run(state, stateValue, default);
    }
}
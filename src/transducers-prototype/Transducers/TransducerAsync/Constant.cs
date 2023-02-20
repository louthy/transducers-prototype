#nullable enable
namespace LanguageExt;

record ConstantTransducerAsync<A, B>(B Value) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(Value, reduce);

    internal record Reduce<S>(B Value, ReducerAsync<S, B> reduce) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A _) =>
            reduce.Run(st, s, Value);
    }
}

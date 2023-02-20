#nullable enable
namespace LanguageExt;

record ConstantTransducer<A, B>(B Value) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Reduce<S>(Value, reduce);

    public override TransducerAsync<A, B> ToAsync() =>
        new ConstantTransducerAsync<A, B>(Value);

    record Reduce<S>(B Value, Reducer<S, B> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A _) =>
            Reducer.Run(st, s, Value);

        public override ReducerAsync<S, A> ToAsync() =>
            new ConstantTransducerAsync<A, B>.Reduce<S>(Value, Reducer.ToAsync());
    }
}

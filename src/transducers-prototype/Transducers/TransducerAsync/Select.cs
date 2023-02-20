#nullable enable
namespace LanguageExt;

record SelectTransducerAsync<A, B, C>(TransducerAsync<A, B> F, Func<B, C> G) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(F, G, reduce);

    internal record Reduce<S>(TransducerAsync<A, B> F, Func<B, C> G, ReducerAsync<S, C> Reducer) : 
        ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            F.Transform(new Mapper<S>(G, Reducer)).Run(st, s, x);
    }

    internal record Mapper<S>(Func<B, C> G, ReducerAsync<S, C> Reducer) :
        ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B b) =>
            Reducer.Run(st, s, G(b));
    }
}

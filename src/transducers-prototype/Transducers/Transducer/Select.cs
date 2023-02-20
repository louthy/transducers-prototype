﻿#nullable enable
namespace LanguageExt;

record SelectTransducer<A, B, C>(Transducer<A, B> F, Func<B, C> G) : 
    Transducer<A, C>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(F, G, reduce);

    public override TransducerAsync<A, C> ToAsync() =>
        throw new NotImplementedException();

    record Reduce<S>(Transducer<A, B> F, Func<B, C> G, Reducer<S, C> Reducer) : 
        Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            F.Transform(new Mapper<S>(G, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, A> ToAsync() =>
            new SelectTransducerAsync<A, B, C>.Reduce<S>(F.ToAsync(), G, Reducer.ToAsync());
    }

    record Mapper<S>(Func<B, C> G, Reducer<S, C> Reducer) :
        Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B b) =>
            Reducer.Run(st, s, G(b));

        public override ReducerAsync<S, B> ToAsync() =>
            new SelectTransducerAsync<A, B, C>.Mapper<S>(G, Reducer.ToAsync());
    }
}

#nullable enable
namespace LanguageExt;

record FlattenTransducer1<A, B>(Transducer<A, Transducer<A, B>> FF) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Reduce<S>(FF, reduce);

    public override TransducerAsync<A, B> ToAsync() =>
        new FlattenTransducerAsyncSync1<A, B>(FF.ToAsync());
    
    record Reduce<S>(Transducer<A, Transducer<A, B>> FF, Reducer<S, B> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, A> ToAsync() =>
            new FlattenTransducerAsyncSync1<A, B>.Reduce<S>(FF.ToAsync(), Reducer.ToAsync());
    }    
    
    record Reduce1<S>(A Value, Reducer<S, B> Reducer) : Reducer<S, Transducer<A, B>>
    {
        public override TResult<S> Run(TState st, S s, Transducer<A, B> f) =>
            f.Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, Transducer<A, B>> ToAsync() =>
            new FlattenTransducerAsyncSync1<A, B>.Reduce1<S>(Value, Reducer.ToAsync());
    }    
}

record FlattenTransducer2<A, B>(Transducer<A, Transducer<Unit, B>> FF) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Reduce<S>(FF, reduce);

    public override TransducerAsync<A, B> ToAsync() =>
        new FlattenTransducerAsyncSync2<A, B>(FF.ToAsync());
    
    record Reduce<S>(Transducer<A, Transducer<Unit, B>> FF, Reducer<S, B> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            FF.Transform(new Reduce1<S>(Reducer)).Run(st, s, x);

        public override ReducerAsync<S, A> ToAsync() =>
            new FlattenTransducerAsyncSync2<A, B>.Reduce<S>(FF.ToAsync(), Reducer.ToAsync());
    }    
    
    record Reduce1<S>(Reducer<S, B> Reducer) : Reducer<S, Transducer<Unit, B>>
    {
        public override TResult<S> Run(TState st, S s, Transducer<Unit, B> f) =>
            f.Transform(Reducer).Run(st, s, default);

        public override ReducerAsync<S, Transducer<Unit, B>> ToAsync() =>
            new FlattenTransducerAsyncSync2<A, B>.Reduce1<S>(Reducer.ToAsync());
    }    
}

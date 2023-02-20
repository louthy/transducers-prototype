#nullable enable
namespace LanguageExt;

record FlattenTransducerAsync1<A, B>(TransducerAsync<A, TransducerAsync<A, B>> FF) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(TransducerAsync<A, TransducerAsync<A, B>> FF, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(A Value, ReducerAsync<S, B> Reducer) : 
        ReducerAsync<S, TransducerAsync<A, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, TransducerAsync<A, B> f) =>
            new (TResultAsync.Recursive(st, s, Value, f.Transform(Reducer)));
    }    
}

record FlattenTransducerAsyncSync1<A, B>(TransducerAsync<A, Transducer<A, B>> FF) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(TransducerAsync<A, Transducer<A, B>> FF, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(A Value, ReducerAsync<S, B> Reducer) : 
        ReducerAsync<S, Transducer<A, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Transducer<A, B> f) =>
            new (TResultAsync.Recursive(st, s, Value, f.ToAsync().Transform(Reducer)));
    }    
}

record FlattenTransducerAsync2<A, B>(TransducerAsync<A, TransducerAsync<Unit, B>> FF) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(TransducerAsync<A, TransducerAsync<Unit, B>> FF, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            FF.Transform(new Reduce1<S>(Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(ReducerAsync<S, B> Reducer) : ReducerAsync<S, TransducerAsync<Unit, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, TransducerAsync<Unit, B> f) =>
            f.Transform(Reducer).Run(st, s, default);
    }    
}

record FlattenTransducerAsyncSync2<A, B>(TransducerAsync<A, Transducer<Unit, B>> FF) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(TransducerAsync<A, Transducer<Unit, B>> FF, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            FF.Transform(new Reduce1<S>(Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(ReducerAsync<S, B> Reducer) : ReducerAsync<S, Transducer<Unit, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Transducer<Unit, B> f) =>
            f.ToAsync().Transform(Reducer).Run(st, s, default);
    }    
}

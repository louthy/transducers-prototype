#nullable enable
namespace LanguageExt;

record BindTransducer1<A, B, C>(Transducer<A, B> M, Transducer<B, Transducer<A, C>> F) : 
    Transducer<A, C>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    public override TransducerAsync<A, C> ToAsync() =>
        new BindTransducerAsyncSync1<A, B, C>(M.ToAsync(), F.ToAsync());
    
    record Reduce<S>(Transducer<A, B> M, Transducer<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, A> ToAsync() =>
            new BindTransducerAsyncSync1<A, B, C>.Reduce<S>(M.ToAsync(), F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder1<S>(A Value, Transducer<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, Reducer)));

        public override ReducerAsync<S, B> ToAsync() =>
            new BindTransducerAsyncSync1<A, B, C>.Binder1<S>(Value, F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder2<S>(A Value, Reducer<S, C> Reducer) : Reducer<S, Transducer<A, C>>
    {
        public override TResult<S> Run(TState st, S s, Transducer<A, C> f) =>
            f.Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, Transducer<A, C>> ToAsync() =>
            new BindTransducerAsyncSync1<A, B, C>.Binder2<S>(Value, Reducer.ToAsync());
    }
}

record BindTransducer2<A, B, C>(Transducer<A, B> M, Transducer<B, Func<A, C>> F) : 
    Transducer<A, C>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    public override TransducerAsync<A, C> ToAsync() =>
        new BindTransducerAsync2<A, B, C>(M.ToAsync(), F.ToAsync());
    
    record Reduce<S>(Transducer<A, B> M, Transducer<B, Func<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, A> ToAsync() =>
            new BindTransducerAsync2<A, B, C>.Reduce<S>(M.ToAsync(), F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder1<S>(A Value, Transducer<B, Func<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, Reducer)));

        public override ReducerAsync<S, B> ToAsync() =>
            new BindTransducerAsync2<A, B, C>.Binder1<S>(Value, F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder2<S>(A Value, Reducer<S, C> Reducer) : Reducer<S, Func<A, C>>
    {
        public override TResult<S> Run(TState st, S s, Func<A, C> f) =>
            Reducer.Run(st, s, f(Value));

        public override ReducerAsync<S, Func<A, C>> ToAsync() =>
            new BindTransducerAsync2<A, B, C>.Binder2<S>(Value, Reducer.ToAsync());
    }
}

record BindTransducer3<A, B, C>(Transducer<A, B> M, Func<B, Transducer<A, C>> F) : 
    Transducer<A, C>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    public override TransducerAsync<A, C> ToAsync() =>
        new BindTransducerAsync3<A, B, C>(M.ToAsync(), x => F(x).ToAsync());

    internal record Reduce<S>(Transducer<A, B> M, Func<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, A> ToAsync() =>
            new BindTransducerAsync3<A, B, C>.Reduce<S>(M.ToAsync(), x => F(x).ToAsync(), Reducer.ToAsync());
    }
    
    internal record Binder<S>(A Value, Func<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, Value, F(value).Transform(Reducer));

        public override ReducerAsync<S, B> ToAsync() =>
            new BindTransducerAsync3<A, B, C>.Binder<S>(Value, x => F(x).ToAsync(), Reducer.ToAsync());
    }
}

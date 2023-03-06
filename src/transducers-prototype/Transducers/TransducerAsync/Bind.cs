#nullable enable
namespace LanguageExt;

record BindTransducerAsync1<A, B, C>(TransducerAsync<A, B> M, TransducerAsync<B, TransducerAsync<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    record Reduce<S>(TransducerAsync<A, B> M, TransducerAsync<B, TransducerAsync<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    record Binder1<S>(A Value, TransducerAsync<B, TransducerAsync<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, ReducerAsync)));
    }
    
    record Binder2<S>(A Value, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, TransducerAsync<A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, TransducerAsync<A, C> f) =>
            f.Transform(ReducerAsync).Run(st, s, Value);
    }
}

record BindTransducerAsyncSync1<A, B, C>(TransducerAsync<A, B> M, TransducerAsync<B, Transducer<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(TransducerAsync<A, B> M, TransducerAsync<B, Transducer<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    internal record Binder1<S>(A Value, TransducerAsync<B, Transducer<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, ReducerAsync)));
    }
    
    internal record Binder2<S>(A Value, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, Transducer<A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Transducer<A, C> f) =>
            f.ToAsync().Transform(ReducerAsync).Run(st, s, Value);
    }
}

record BindTransducerAsync2<A, B, C>(TransducerAsync<A, B> M, TransducerAsync<B, Func<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(TransducerAsync<A, B> M, TransducerAsync<B, Func<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    internal record Binder1<S>(A Value, TransducerAsync<B, Func<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, ReducerAsync)));
    }
    
    internal record Binder2<S>(A Value, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, Func<A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Func<A, C> f) =>
            ReducerAsync.Run(st, s, f(Value));
    }
}

record BindTransducerAsync3<A, B, C>(TransducerAsync<A, B> M, Func<B, TransducerAsync<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(TransducerAsync<A, B> M, Func<B, TransducerAsync<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    internal record Binder<S>(A Value, Func<B, TransducerAsync<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, Value, F(value).Transform(ReducerAsync));
    }
}

record BindTransducerAsyncSync3<A, B, C>(TransducerAsync<A, B> M, Func<B, Transducer<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(TransducerAsync<A, B> M, Func<B, Transducer<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    internal record Binder<S>(A Value, Func<B, Transducer<A, C>> F, ReducerAsync<S, C> ReducerAsync) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, Value, F(value).ToAsync().Transform(ReducerAsync));
    }
}

record BindTransducerAsync3A<A, B, C>(TransducerAsync<A, B> M, Func<B, TransducerAsync<Unit, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(TransducerAsync<A, B> M, Func<B, TransducerAsync<Unit, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(F, Reducer)).Run(st, s, value);
    }
    
    internal record Binder<S>(Func<B, TransducerAsync<Unit, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, default, F(value).Transform(Reducer));
    }
}

record BindTransducerAsyncSync3A<A, B, C>(TransducerAsync<A, B> M, Func<B, Transducer<Unit, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(TransducerAsync<A, B> M, Func<B, Transducer<Unit, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(F, Reducer)).Run(st, s, value);
    }
    
    internal record Binder<S>(Func<B, Transducer<Unit, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, default, F(value).ToAsync().Transform(Reducer));
    }
}

record BindTransducerAsync3B<A, B, C>(TransducerAsync<Unit, B> M, Func<B, TransducerAsync<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(TransducerAsync<Unit, B> M, Func<B, TransducerAsync<A, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, Reducer)).Run(st, s, default);
    }
    
    internal record Binder<S>(A Value, Func<B, TransducerAsync<A, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, Value, F(value).Transform(Reducer));
    }
}

record BindTransducerAsyncSync3B<A, B, C>(TransducerAsync<Unit, B> M, Func<B, Transducer<A, C>> F) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(TransducerAsync<Unit, B> M, Func<B, Transducer<A, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, Reducer)).Run(st, s, default);
    }
    
    internal record Binder<S>(A Value, Func<B, Transducer<A, C>> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, Value, F(value).ToAsync().Transform(Reducer));
    }
}

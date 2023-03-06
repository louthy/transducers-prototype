#nullable enable
namespace LanguageExt;

record SumBindTransducerAsync1<X, Y, A, B, C>(
    TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
    TransducerAsync<B, SumTransducerAsync<X, Y, A, C>> F) : 
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    record Reduce<S>(
        TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
        TransducerAsync<B, SumTransducerAsync<X, Y, A, C>> F, ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        TransducerAsync<B, SumTransducerAsync<X, Y, A, C>> F, 
        ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResultAsync.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, ReducerAsync))),

                SumLeft<Y, B> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
    
    record Binder2<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, SumTransducerAsync<X, Y, A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, SumTransducerAsync<X, Y, A, C> f) =>
            f.Transform(ReducerAsync).Run(st, s, Value);
    }
}

record SumBindTransducerAsyncSync1<X, Y, A, B, C>(
    TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
    TransducerAsync<B, SumTransducer<X, Y, A, C>> F) : 
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(
            TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
            TransducerAsync<B, SumTransducer<X, Y, A, C>> F, ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    internal record Binder1<S>(
            Sum<X, A> Value, 
            TransducerAsync<B, SumTransducer<X, Y, A, C>> F, 
            ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResultAsync.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, ReducerAsync))),

                SumLeft<Y, B> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
    
    internal record Binder2<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, C>> ReducerAsync) :
        ReducerAsync<S, SumTransducer<X, Y, A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, SumTransducer<X, Y, A, C> f) =>
            f.ToAsync().Transform(ReducerAsync).Run(st, s, Value);
    }
}

record SumBindTransducerAsync2<X, Y, A, B, C>(
    TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
    TransducerAsync<B, Func<Sum<X, A>, Sum<Y, C>>> F) : 
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(
        TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
        TransducerAsync<B, Func<Sum<X, A>, Sum<Y, C>>> F, ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    internal record Binder1<S>(
        Sum<X, A> Value, 
        TransducerAsync<B, Func<Sum<X, A>, Sum<Y, C>>> F, 
        ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResultAsync.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, Reducer))),

                SumLeft<Y, B> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
    
    internal record Binder2<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, C>> Reducer) : ReducerAsync<S, Func<Sum<X, A>, Sum<Y, C>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Func<Sum<X, A>, Sum<Y, C>> f) =>
            Reducer.Run(st, s, f(Value));
    }
}

record SumBindTransducerAsync3<X, Y, A, B, C>(
    TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
    Func<B, TransducerAsync<Sum<X, A>, Sum<Y, C>>> F) : 
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    internal record Reduce<S>(
            TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
            Func<B, TransducerAsync<Sum<X, A>, Sum<Y, C>>> F, ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, ReducerAsync)).Run(st, s, value);
    }
    
    internal record Binder1<S>(
            Sum<X, A> Value, 
            Func<B, TransducerAsync<Sum<X, A>, Sum<Y, C>>> F, 
            ReducerAsync<S, Sum<Y, C>> ReducerAsync) 
        : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResultAsync.Recursive(st, s, Value, F(r.Value).Transform(ReducerAsync)),

                SumLeft<Y, B> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
}

record SumBindTransducerAsyncSync3A<X, Y, A, B, C>(
    TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
    Func<B, Transducer<A, C>> F) : 
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(
            TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
            Func<B, Transducer<A, C>> F, 
            ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> r =>
                    M.Transform(new Binder1<S>(r.Value, F, Reducer)).Run(st, s, value),

                SumLeft<X, A> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
    
    internal record Binder1<S>(
            A Value, 
            Func<B, Transducer<A, C>> F, 
            ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResultAsync.Recursive(st, s, Value, F(r.Value).ToAsync().Transform(new MapRight<S>(Reducer))),

                SumLeft<Y, B> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
        
    internal record MapRight<S>(ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, C>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, C value) =>
            Reducer.Run(st, s, Sum<Y, C>.Right(value));
    }
}

record SumBindTransducerAsync3A<X, Y, A, B, C>(
    TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
    Func<B, TransducerAsync<A, C>> F) : 
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(
            TransducerAsync<Sum<X, A>, Sum<Y, B>> M, 
            Func<B, TransducerAsync<A, C>> F, 
            ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> r =>
                    M.Transform(new Binder1<S>(r.Value, F, Reducer)).Run(st, s, value),

                SumLeft<X, A> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
    
    internal record Binder1<S>(
            A Value, 
            Func<B, TransducerAsync<A, C>> F, 
            ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResultAsync.Recursive(st, s, Value, F(r.Value).Transform(new MapRight<S>(Reducer))),

                SumLeft<Y, B> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }
        
    internal record MapRight<S>(ReducerAsync<S, Sum<Y, C>> Reducer) 
        : ReducerAsync<S, C>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, C value) =>
            Reducer.Run(st, s, Sum<Y, C>.Right(value));
    }
}

record SumBindTransducerAsync3B<X, Y, A, B, C>(
    TransducerAsync<A, B> M,
    Func<B, TransducerAsync<Sum<X, A>, Sum<Y, C>>> F) :
    SumTransducerAsync<X, Y, A, C>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(
            TransducerAsync<A, B> M,
            Func<B, TransducerAsync<Sum<X, A>, Sum<Y, C>>> F, ReducerAsync<S, Sum<Y, C>> Reducer)
        : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> r =>
                    M.Transform(new Binder1<S>(r.Value, F, Reducer)).Run(st, s, r.Value),

                SumLeft<X, A> =>
                    TResultAsync.Continue(s),

                _ =>
                    TResultAsync.Complete(s)
            };
    }

    internal record Binder1<S>(
            A Value,
            Func<B, TransducerAsync<Sum<X, A>, Sum<Y, C>>> F,
            ReducerAsync<S, Sum<Y, C>> Reducer)
        : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B value) =>
            TResultAsync.Recursive(st, s, Sum<X, A>.Right(Value), F(value).Transform(Reducer));
    }
}

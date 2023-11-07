/*
#nullable enable
using LanguageExt.Examples;
using LanguageExt.HKT;

namespace LanguageExt;

record SumBindTransducer1<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Transducer<B, SumTransducer<X, Y, A, C>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
        
    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, B>> M, 
        Transducer<B, SumTransducer<X, Y, A, C>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        Transducer<B, SumTransducer<X, Y, A, C>> F, 
        Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, Reducer))),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };
    }
    
    record Binder2<S>(Sum<X, A> Value, Reducer<S, Sum<Y, C>> Reducer) : Reducer<S, SumTransducer<X, Y, A, C>>
    {
        public override TResult<S> Run(TState st, S s, SumTransducer<X, Y, A, C> f) =>
            f.Transform(Reducer).Run(st, s, Value);
    }
}

record SumBindTransducer2<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, B>> M, 
        Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> F, 
        Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, Reducer))),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };
    }
    
    record Binder2<S>(Sum<X, A> Value, Reducer<S, Sum<Y, C>> Reducer) : Reducer<S, Func<Sum<X, A>, Sum<Y, C>>>
    {
        public override TResult<S> Run(TState st, S s, Func<Sum<X, A>, Sum<Y, C>> f) =>
            Reducer.Run(st, s, f(Value));
    }
}

record SumBindTransducer3<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, B>> M, 
        Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F, 
        Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, Value, F(r.Value).Transform(Reducer)),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };
    }
}

record SumBindTransducer3A<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Func<B, Transducer<A, C>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    record Reduce<S>(
            Transducer<Sum<X, A>, Sum<Y, B>> M, 
            Func<B, Transducer<A, C>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> r =>
                    M.Transform(new Binder1<S>(r.Value, F, Reducer)).Run(st, s, value),

                SumLeft<X, A> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };
    }
    
    record Binder1<S>(
            A Value, 
            Func<B, Transducer<A, C>> F, 
            Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, Value, F(r.Value).Transform(new MapRight<S>(Reducer))),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };
    }
        
    record MapRight<S>(Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, C>
    {
        public override TResult<S> Run(TState st, S s, C value) =>
            Reducer.Run(st, s, Sum<Y, C>.Right(value));
    }
}

record SumBindTransducer3B<X, Y, A, B, C>(
    Transducer<A, B> M,
    Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F) :
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    record Reduce<S>(
            Transducer<A, B> M,
            Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F, Reducer<S, Sum<Y, C>> Reducer)
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            value switch
            {
                SumRight<X, A> r =>
                    M.Transform(new Binder1<S>(r.Value, F, Reducer)).Run(st, s, r.Value),

                SumLeft<X, A> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };
    }

    record Binder1<S>(
            A Value,
            Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F,
            Reducer<S, Sum<Y, C>> Reducer)
        : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, Sum<X, A>.Right(Value), F(value).Transform(Reducer));
    }
}
*/

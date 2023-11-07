﻿#nullable enable
namespace LanguageExt;

record BindTransducer1<A, B, C>(Transducer<A, B> M, Transducer<B, Transducer<A, C>> F) : 
    Transducer<A, C>
{
    public Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    record Reduce<S>(Transducer<A, B> M, Transducer<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    record Binder1<S>(A Value, Transducer<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, Reducer)));
    }
    
    record Binder2<S>(A Value, Reducer<S, C> Reducer) : Reducer<S, Transducer<A, C>>
    {
        public override TResult<S> Run(TState st, S s, Transducer<A, C> f) =>
            f.Transform(Reducer).Run(st, s, Value);
    }

    public Transducer<A, C> Morphism =>
        this;
}

record BindTransducer2<A, B, C>(Transducer<A, B> M, Transducer<B, Func<A, C>> F) : 
    Transducer<A, C>
{
    public Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);
    
    record Reduce<S>(Transducer<A, B> M, Transducer<B, Func<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    record Binder1<S>(A Value, Transducer<B, Func<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, value, F.Transform(new Binder2<S>(Value, Reducer)));
    }
    
    record Binder2<S>(A Value, Reducer<S, C> Reducer) : Reducer<S, Func<A, C>>
    {
        public override TResult<S> Run(TState st, S s, Func<A, C> f) =>
            Reducer.Run(st, s, f(Value));
    }

    public Transducer<A, C> Morphism =>
        this;
}

record BindTransducer3<A, B, C>(Transducer<A, B> M, Func<B, Transducer<A, C>> F) : 
    Transducer<A, C>
{
    public Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(Transducer<A, B> M, Func<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, Reducer)).Run(st, s, value);
    }
    
    internal record Binder<S>(A Value, Func<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, Value, F(value).Transform(Reducer));
    }

    public Transducer<A, C> Morphism =>
        this;
}

record BindTransducer3A<A, B, C>(Transducer<A, B> M, Func<B, Transducer<Unit, C>> F) : 
    Transducer<A, C>
{
    public Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(Transducer<A, B> M, Func<B, Transducer<Unit, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(F, Reducer)).Run(st, s, value);
    }
    
    internal record Binder<S>(Func<B, Transducer<Unit, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, default, F(value).Transform(Reducer));
    }

    public Transducer<A, C> Morphism =>
        this;
}

record BindTransducer3B<A, B, C>(Transducer<Unit, B> M, Func<B, Transducer<A, C>> F) : 
    Transducer<A, C>
{
    public Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(M, F, reduce);

    internal record Reduce<S>(Transducer<Unit, B> M, Func<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            M.Transform(new Binder<S>(value, F, Reducer)).Run(st, s, default);
    }
    
    internal record Binder<S>(A Value, Func<B, Transducer<A, C>> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, Value, F(value).Transform(Reducer));
    }

    public Transducer<A, C> Morphism =>
        this;
}

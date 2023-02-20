#nullable enable
namespace LanguageExt;

record LiftTransducer1<A, B>(Func<A, TResult<B>> F) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    public override TransducerAsync<A, B> ToAsync() =>
        new LiftTransducerAsync1<A, B>(x => F(x).ToAsync());

    record Reduce<S>(Func<A, TResult<B>> F, Reducer<S, B> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            F(x).Reduce(st, s, Reducer);

        public override ReducerAsync<S, A> ToAsync() =>
            new LiftTransducerAsync1<A, B>.Reduce<S>(x => F(x).ToAsync(), Reducer.ToAsync());
    }
}

record LiftTransducer2<A>(Func<TResult<A>> F) : Transducer<Unit, A>
{
    public override Reducer<S, Unit> Transform<S>(Reducer<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    public override TransducerAsync<Unit, A> ToAsync() =>
        new LiftTransducerAsync2<A>(() => F().ToAsync());

    record Reduce<S>(Func<TResult<A>> F, Reducer<S, A> Reducer) : Reducer<S, Unit>
    {
        public override TResult<S> Run(TState st, S s, Unit x) =>
            F().Reduce(st, s, Reducer);

        public override ReducerAsync<S, Unit> ToAsync() =>
            new LiftTransducerAsync2<A>.Reduce<S>(() => F().ToAsync(), Reducer.ToAsync());
    }
}

record LiftTransducer3<A, B>(Func<A, B> F) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    public override TransducerAsync<A, B> ToAsync() =>
        new LiftTransducerAsync3<A, B>(F);

    record Reduce<S>(Func<A, B> F, Reducer<S, B> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            Reducer.Run(st, s, F(x));
 
        public override ReducerAsync<S, A> ToAsync() =>
            new LiftTransducerAsync3<A, B>.Reduce<S>(F, Reducer.ToAsync());
    }
}

record LiftTransducer4<A>(Func<A> F) : Transducer<Unit, A>
{
    public override Reducer<S, Unit> Transform<S>(Reducer<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    public override TransducerAsync<Unit, A> ToAsync() =>
        new LiftTransducerAsync4<A>(F);

    record Reduce<S>(Func<A> F, Reducer<S, A> Reducer) : Reducer<S, Unit>
    {
        public override TResult<S> Run(TState st, S s, Unit x) =>
            Reducer.Run(st, s, F());
 
        public override ReducerAsync<S, Unit> ToAsync() =>
            new LiftTransducerAsync4<A>.Reduce<S>(F, Reducer.ToAsync());
    }
}
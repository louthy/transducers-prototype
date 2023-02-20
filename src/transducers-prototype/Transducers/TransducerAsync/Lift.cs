#nullable enable
namespace LanguageExt;

record LiftTransducerAsync1<A, B>(Func<A, TResultAsync<B>> F) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A, TResultAsync<B>> F, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            F(x).Reduce(st, s, Reducer);
    }
}

record LiftTransducerAsync2<A>(Func<TResultAsync<A>> F) : TransducerAsync<Unit, A>
{
    public override ReducerAsync<S, Unit> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<TResultAsync<A>> F, ReducerAsync<S, A> Reducer) : ReducerAsync<S, Unit>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Unit x) =>
            F().Reduce(st, s, Reducer);
    }
}

record LiftTransducerAsync3<A, B>(Func<A, B> F) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A, B> F, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            Reducer.Run(st, s, F(x));
    }
}

record LiftTransducerAsync4<A>(Func<A> F) : TransducerAsync<Unit, A>
{
    public override ReducerAsync<S, Unit> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A> F, ReducerAsync<S, A> Reducer) : ReducerAsync<S, Unit>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Unit x) =>
            Reducer.Run(st, s, F());
    }
}

record LiftTransducerAsync5<A, B>(Func<A, ValueTask<B>> F) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A, ValueTask<B>> F, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            await Reducer.Run(st, s, await F(x).ConfigureAwait(false)).ConfigureAwait(false);
    }
}

record LiftTransducerAsync6<A>(Func<ValueTask<A>> F) : TransducerAsync<Unit, A>
{
    public override ReducerAsync<S, Unit> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<ValueTask<A>> F, ReducerAsync<S, A> Reducer) : ReducerAsync<S, Unit>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, Unit x) =>
            await Reducer.Run(st, s, await F().ConfigureAwait(false)).ConfigureAwait(false);
    }
}

record LiftTransducerAsync7<A, B>(Func<A, Task<B>> F) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A, Task<B>> F, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            await Reducer.Run(st, s, await F(x).ConfigureAwait(false)).ConfigureAwait(false);
    }
}

record LiftTransducerAsync8<A>(Func<Task<A>> F) : TransducerAsync<Unit, A>
{
    public override ReducerAsync<S, Unit> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<Task<A>> F, ReducerAsync<S, A> Reducer) : ReducerAsync<S, Unit>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, Unit x) =>
            await Reducer.Run(st, s, await F().ConfigureAwait(false)).ConfigureAwait(false);
    }
}

record LiftTransducerAsync9<A, B>(Func<A, ValueTask<TResultAsync<B>>> F) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A, ValueTask<TResultAsync<B>>> F, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            await (await F(x).ConfigureAwait(false)).Reduce(st, s, Reducer).ConfigureAwait(false);
    }
}

record LiftTransducerAsync10<A>(Func<ValueTask<TResultAsync<A>>> F) : TransducerAsync<Unit, A>
{
    public override ReducerAsync<S, Unit> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<ValueTask<TResultAsync<A>>> F, ReducerAsync<S, A> Reducer) : ReducerAsync<S, Unit>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, Unit x) =>
            await (await F().ConfigureAwait(false)).Reduce(st, s, Reducer).ConfigureAwait(false);
    }
}

record LiftTransducerAsync11<A, B>(Func<A, Task<TResultAsync<B>>> F) : TransducerAsync<A, B>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<A, Task<TResultAsync<B>>> F, ReducerAsync<S, B> Reducer) : ReducerAsync<S, A>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            await (await F(x).ConfigureAwait(false)).Reduce(st, s, Reducer).ConfigureAwait(false);
    }
}

record LiftTransducerAsync12<A>(Func<Task<TResultAsync<A>>> F) : TransducerAsync<Unit, A>
{
    public override ReducerAsync<S, Unit> Transform<S>(ReducerAsync<S, A> reduce) =>
        new Reduce<S>(F, reduce);

    internal record Reduce<S>(Func<Task<TResultAsync<A>>> F, ReducerAsync<S, A> Reducer) : ReducerAsync<S, Unit>
    {
        public override async ValueTask<TResultAsync<S>> Run(TState st, S s, Unit x) =>
            await (await F().ConfigureAwait(false)).Reduce(st, s, Reducer).ConfigureAwait(false);
    }
}
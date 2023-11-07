namespace LanguageExt.Examples;

public struct Eff<Env, E, A> : Transducer<Env, Sum<E, A>>
{
    readonly Transducer<Env, Sum<E, A>> morphism;

    Eff(Transducer<Env, Sum<E, A>> morphism) =>
        this.morphism = morphism;
    
    public static Eff<Env, E, A> Success(A value) =>
        new (Transducer.constant<Env, Sum<E, A>>(Sum<E, A>.Right(value)));
    
    public static Eff<Env, E, A> Fail(E value) =>
        new (Transducer.constant<Env, Sum<E, A>>(Sum<E, A>.Left(value)));
    
    public TResult<Sum<E, A>> Run(Env env) =>
        Morphism.Invoke1(env);
    
    public Eff<Env, E, B> Map<B>(Func<A, B> f) =>
        new(Transducer.mapRight(Morphism, Transducer.lift(f)));

    public Eff<Env, E, A> MapError(Func<E, E> f) =>
        new(Transducer.mapLeft(Morphism, Transducer.lift(f)));

    public Eff<Env, E, B> Select<B>(Func<A, B> f) =>
        new(Transducer.mapRight(Morphism, Transducer.lift(f)));

    public Eff<Env, E, B> Bind<B>(Func<A, Eff<Env, E, B>> f) =>
        new (Map(x => f(x).Morphism).Morphism.Flatten());

    public Eff<Env, E, C> SelectMany<B, C>(Func<A, Eff<Env, E, B>> bind, Func<A, B, C> f) =>
        Bind(x => bind(x).Map(y => f(x, y)));

    public Eff<Env, E, B> BiMap<B>(Func<E, E> Fail, Func<A, B> Succ) =>
        new(Transducer.bimap(Morphism, Transducer.lift(Fail), Transducer.lift(Succ)));

    public Eff<Env, E, B> BiMap<B>(Transducer<E, E> Fail, Transducer<A, B> Succ) =>
        new(Transducer.bimap(Morphism, Fail, Succ));

    public Transducer<Env, Sum<E, A>> Morphism =>
        morphism ?? throw new Exception("Eff not intialised");

    public Reducer<S, Env> Transform<S>(Reducer<S, Sum<E, A>> reduce) =>
        Morphism.Transform(reduce);
}

public static class Eff
{
    public static Eff<Env, E, A> Flatten<Env, E, A>(this Eff<Env, E, Eff<Env, E, A>> mma) =>
        mma.Bind(static mx => mx);
}
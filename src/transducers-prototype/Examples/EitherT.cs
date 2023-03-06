using LanguageExt.HKT;

namespace LanguageExt.Examples;

/// <summary>
/// Either transformer
/// </summary>
public record EitherT<M, Env, L, R>(Transducer<Env, SumTransducer<Unit, L, Unit, R>> MorphismValue) : 
    KArr<M, Env, SumTransducer<Unit, L, Unit, R>>
    where M : Monad<M, Env>
{
    public Transducer<Env, SumTransducer<Unit, L, Unit, R>> Morphism => 
        MorphismValue;

    public static EitherT<M, Env, L, R> Right(R value) =>
        new (M.Pure(SumTransducer.Right<L, R>(value)).Morphism);

    public static EitherT<M, Env, L, R> Left(L value) =>
        new (M.Pure(SumTransducer.Left<L, R>(value)).Morphism);
    
    public static implicit operator EitherT<M, Env, L, R>(Transducer<Env, SumTransducer<Unit, L, Unit, R>> m) =>
        new (m);
    
    public EitherT<M, Env, L, B> Map<B>(Func<R, B> f) =>
        Morphism.Map(mx => mx.Map(f));

    public EitherT<M, Env, L, B> Select<B>(Func<R, B> f) =>
        Morphism.Map(mx => mx.Map(f));

    public EitherT<M, Env, L, B> Bind<B>(Func<R, EitherT<M, Env, L, B>> f) =>
        Morphism
            .Map(mx => mx.Map(f))
            .Flatten()
            .Map(mx => mx.Morphism)
            .Flatten();

    public EitherT<M, Env, L, C> SelectMany<B, C>(Func<R, EitherT<M, Env, L, B>> bind, Func<R, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
}

/*
public readonly struct EitherT<M, Env, L> : MonadSum2<M, Env, L>
    where M : MonadSum2<M, Env, L>
{
}
*/

/*using LanguageExt.HKT;

namespace LanguageExt.Examples;

/*public abstract record Maybe;

/// <summary>
/// Maybe data-type
/// </summary>
public abstract record Maybe<A> : Maybe
{
    public static Maybe<A> Just(A value) => new JustCase<A>(value);
    public static readonly Maybe<A> Nothing = new NothingCase<A>();
}
public record JustCase<A>(A Value): Maybe<A>;
public record NothingCase<A> : Maybe<A>;#1#

/// <summary>
/// Maybe transformer
/// </summary>
public readonly record struct MaybeT<M, Env, X, A>(SumTransducer<Env, X, Env, SumTransducer<Unit, Unit, Unit, A>> MorphismUnsafe) : 
    KArr<M, Env,X, Env, SumTransducer<Unit, Unit, Unit, A>>
    where M : MonadSum<M, Env, X>
{
    Transducer<Sum<Env, Env>, Sum<X, SumTransducer<Unit, Unit, Unit, A>>> KArr<M, Sum<Env, Env>, Sum<X, SumTransducer<Unit, Unit, Unit, A>>>.Morphism => 
        MorphismUnsafe.Morphism;

    public SumTransducer<Env, X, Env, SumTransducer<Unit, Unit, Unit, A>> Morphism => 
        MorphismUnsafe.Morphism;

    public static MaybeT<M, Env, X, A> Some(A value) =>
        new (M.Pure(SumTransducer.Right<Unit, A>(value)).Morphism);
    
    public static readonly MaybeT<M, Env, X, A> None =
        new (M.Pure(SumTransducer.Left<Unit, A>(default)).Morphism);
    
    public static MaybeT<M, Env, X, A> Optional(A? value) =>
        value is null 
            ? None 
            : Some(value);
    
    public static implicit operator MaybeT<M, Env, X, A>(SumTransducer<Env, X, Env, SumTransducer<Unit, Unit, Unit, A>> m) =>
        new (m);
    
    public static implicit operator MaybeT<M, Env, X, A>(Transducer<Sum<Env, Env>, Sum<X, SumTransducer<Unit, Unit, Unit, A>>> m) =>
        new (SumTransducer.lift(m));
    
    public MaybeT<M, Env, X, B> Map<B>(Func<A, B> f) =>
        Morphism.MapRight(mx => Transducer.MapRight(mx, f));

    public MaybeT<M, Env, X, B> Select<B>(Func<A, B> f) =>
        Morphism.MapRight(mx => Transducer.MapRight(mx, f));

    public MaybeT<M, Env, X, B> Bind<B>(Func<A, MaybeT<M, Env, X, B>> f) =>
        Morphism
            .MapRight(mx => Transducer.MapRight(mx, f))
            .Flatten()
            .MapRight(mx => mx.Morphism)
            .Flatten();
}

public readonly struct MaybeT<M, Env, X> : MonadSum<M, Env, X>
    where M : MonadSum<M, Env, X>
{
    public static KArr<M, Env, X, Env, B> BiMap<A, B>(KArr<M, Env, X, Env, A> fab, Transducer<X, X> Left, Transducer<A, B> Right)
    {
        throw new NotImplementedException();
    }

    public static KArr<M, Env, X, Env, A> Lift<A>(SumTransducer<Env, X, Env, A> f)
    {
        throw new NotImplementedException();
    }

    public static KArr<M, Env, X, Env, B> Bind<A, B>(KArr<M, Env, X, Env, A> mx, Transducer<A, KArr<M, Env, X, Env, B>> f)
    {
        throw new NotImplementedException();
    }
}*/
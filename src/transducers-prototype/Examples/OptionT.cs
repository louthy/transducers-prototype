#nullable enable
using LanguageExt.HKT;

namespace LanguageExt.Examples;

/// <summary>
/// Option transformer
/// </summary>
public readonly struct OptionT<M, A> : KArr<M, Unit, Sum<Unit, A>>
    where M : MonadReader<M, Unit>
{
    readonly Transducer<Unit, Sum<Unit, A>> morphism;
    
    OptionT(Transducer<Unit, Sum<Unit, A>> morphism) =>
        this.morphism = morphism;
    
    public Transducer<Unit, Sum<Unit, A>> Morphism => 
        morphism;

    public static OptionT<M, A> Some(A value) =>
        new (M.Pure(Transducer.Pure(Sum<Unit, A>.Right(value))).Morphism.Flatten());

    public static readonly OptionT<M, A> None =
        new (M.Pure(Transducer.Pure(Sum<Unit, A>.Left(default))).Morphism.Flatten());
    
    public static OptionT<M, A> Lift(Transducer<Unit, A> ma) =>
        Lift(ma.Map(Sum<Unit, A>.Right));
    
    public static OptionT<M, A> Lift(Transducer<Unit, Sum<Unit, A>> ma) =>
        new (M.Pure(ma).Morphism.Flatten());
    
    public OptionT<M, B> Map<B>(Func<A, B> f) =>
        new(Morphism.MapRight(f));

    public OptionT<M, B> Select<B>(Func<A, B> f) =>
        new(Morphism.MapRight(f));

    public OptionT<M, B> Bind<B>(Func<A, OptionT<M, B>> f) =>
        new(Morphism.MapRight(x => f(x).Morphism).Flatten());

    public OptionT<M, C> SelectMany<B, C>(Func<A, OptionT<M, B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));

    public A Match<A>(Func<A, A> Some, Func<A> None, Func<A>? Bottom = null) =>
        Morphism.Invoke1(default) switch
        {
            TComplete<Sum<Unit, A>> inner =>
                inner switch
                {
                    SumRight<Unit, A> right => Some(right.Value),
                    SumLeft<Unit, A> _ => None(),
                    _ => Bottom is null ? throw new NotSupportedException() : Bottom()
                },

            TCancelled<Sum<Unit, A>> inner =>
                throw new OperationCanceledException(),

            TFail<Sum<Unit, A>> inner =>
                inner.Error.Throw<A>(),

            _ => Bottom is null ? throw new NotSupportedException() : Bottom()
        };
}

/// <summary>
/// Option transformer
/// </summary>
public readonly struct OptionT<M, Env, A>: 
    KArr<M, Env, Sum<Unit, A>>
    where M : MonadReader<M, Env>
{
    readonly Transducer<Env, Sum<Unit, A>> morphism;
    
    OptionT(Transducer<Env, Sum<Unit, A>> morphism) =>
        this.morphism = morphism;
    
    public Transducer<Env, Sum<Unit, A>> Morphism => 
        morphism;

    public static OptionT<M, Env, A> Some(A value) =>
        new (M.Pure(Transducer.constant<Env, Sum<Unit, A>>(Sum<Unit, A>.Right(value))).Morphism.Flatten());

    public static readonly OptionT<M, Env, A> None =
        new (M.Pure(Transducer.constant<Env, Sum<Unit, A>>(Sum<Unit, A>.Left(default))).Morphism.Flatten());

    public static OptionT<M, Env, A> Lift(Transducer<Env, A> ma) =>
        new (M.Pure(ma.Map(Sum<Unit, A>.Right)).Morphism.Flatten());

    public static OptionT<M, Env, A> Lift(Transducer<Env, Sum<Unit, A>> ma) =>
        new (M.Pure(ma).Morphism.Flatten());
    
    public OptionT<M, Env, B> Map<B>(Func<A, B> f) =>
        new(Morphism.MapRight(f));

    public OptionT<M, Env, B> Select<B>(Func<A, B> f) =>
        new(Morphism.MapRight(f));

    public OptionT<M, Env, B> Bind<B>(Func<A, OptionT<M, Env, B>> f) =>
        new(Morphism.MapRight(x => f(x).Morphism).Flatten());

    public OptionT<M, Env, C> SelectMany<B, C>(Func<A, OptionT<M, Env, B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));

    public A Match<A>(Env env, Func<A, A> Some, Func<A> None, Func<A>? Bottom = null) =>
        Morphism.Invoke1(env) switch
        {
            TComplete<Sum<Unit, A>> inner =>
                inner switch
                {
                    SumRight<Unit, A> right => Some(right.Value),
                    SumLeft<Unit, A> => None(),
                    _ => Bottom is null ? throw new NotSupportedException() : Bottom()
                },

            TCancelled<Sum<Unit, A>> inner =>
                throw new OperationCanceledException(),

            TFail<Sum<Unit, A>> inner =>
                inner.Error.Throw<A>(),

            _ => Bottom is null ? throw new NotSupportedException() : Bottom()
        };
}
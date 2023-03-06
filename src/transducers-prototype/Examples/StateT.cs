using LanguageExt.HKT;

namespace LanguageExt.Examples;

/*
/// <summary>
/// State transformer
/// </summary>
public record StateT<M, S, X, A>(Transducer<S, Transducer<>> MorphismValue) : 
    KArr<M, S, X, S, SumTransducer<Unit, X, Unit, A>>
    where M : MonadSum<M, S, X>
{
    Transducer<Sum<S, S>, Sum<X, SumTransducer<Unit, X, Unit, A>>> KArr<M, Sum<S, S>, Sum<X, SumTransducer<Unit, X, Unit, A>>>.Morphism => 
        MorphismValue;
 
    public SumTransducer<S, X, S, SumTransducer<Unit, X, Unit, A>> Morphism => 
        MorphismValue;

    public static StateT<M, S, X, A> Right(A value) =>
        new (M.Pure(SumTransducer.Right<X, A>(value)).Morphism);

    public static StateT<M, S, X, A> Left(X value) =>
        new (M.Pure(SumTransducer.Left<X, A>(value)).Morphism);
    
    public static implicit operator StateT<M, S, X, A>(SumTransducer<S, X, S, SumTransducer<Unit, X, Unit, A>> m) =>
        new (m);
    
    public static implicit operator StateT<M, S, X, A>(Transducer<Sum<S, S>, Sum<X, SumTransducer<Unit, X, Unit, A>>> m) =>
        new (SumTransducer.lift(m));
    
    public StateT<M, S, X, B> Map<B>(Func<A, B> f) =>
        Morphism.Map(mx => mx.Map(f));

    public StateT<M, S, X, B> Select<B>(Func<A, B> f) =>
        Morphism.Map(mx => mx.Map(f));

    public StateT<M, S, X, B> Bind<B>(Func<A, StateT<M, S, X, B>> f) =>
        Morphism
            .Map(mx => mx.Map(f))
            .Flatten()
            .Map(mx => mx.Morphism)
            .Flatten();

    public StateT<M, S, X, C> SelectMany<B, C>(Func<A, StateT<M, S, X, B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
}

/*
public readonly struct StateT<M, Env, L> : MonadSum2<M, Env, L>
    where M : MonadSum2<M, Env, L>
{
}
#1#
*/

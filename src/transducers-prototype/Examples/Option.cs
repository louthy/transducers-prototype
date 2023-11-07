/*
using LanguageExt.HKT;

namespace LanguageExt.Examples;

public readonly record struct Option<A>(Transducer<Unit, Sum<Unit, A>> MorphismUnsafe) 
    : KArr<Option, Unit, Sum<Unit, A>>
{
    public Transducer<Unit, Sum<Unit, A>> Morphism => 
        MorphismUnsafe;
    
    public static Option<A> Some(A value) =>
        new(Transducer.Pure(Sum<Unit, A>.Right(value)));
    
    public static readonly Option<A> None =
        new(Transducer.Pure(Sum<Unit, A>.Left(default)));
    
    public static Option<A> Optional(A? value) =>
        value is null 
            ? None 
            : Some(value);
    
    public Option<B> Map<B>(Func<A, B> f) =>
        new(Morphism.MapRight(f));

    public Option<B> Select<B>(Func<A, B> f) =>
        new(Morphism.MapRight(f));

    public Option<B> Bind<B>(Func<A, Option<B>> f) =>
        new (Map(x => f(x).Morphism).Morphism.Flatten());

    public Option<C> SelectMany<B, C>(Func<A, Option<B>> b, Func<A, B, C> p) =>
        Bind(x => b(x).Map(y => p(x, y)));

    public B Match<B>(Func<A, B> Some, Func<B> None) =>
        Morphism.Invoke1(Sum<Unit, Unit>.Right(default)) switch
        {
            TComplete<Sum<Unit, A>> {Value: SumRight<Unit, A> r} => Some(r.Value),
            TFail<Sum<Unit, A>> f                                => throw new Exception(f.Error.Message),     // Placeholder 
            TCancelled<Sum<Unit, A>>                             => throw new OperationCanceledException(), 
            _                                                    => None()
        };

}

/*
public readonly struct Option : 
    MonadSum<Option, Unit, Unit>, 
    ApplySum<Option, Unit, Unit>
{
    public static KArr<Option, Unit, Unit, Unit, A> Lift<A>(SumTransducer<Unit, Unit, Unit, A> f) =>
        new Option<A>(f);

    public static KArr<Option, Unit, Unit, Unit, B> BiMap<A, B>(
        KArr<Option, Unit, Unit, Unit, A> fab, 
        Transducer<Unit, Unit> Left, 
        Transducer<A, B> Right) =>
        new Option<B>(SumTransducer.compose(fab.Morphism, SumTransducer.bimap(Left, Right)));

    public static KArr<Option, Unit, Unit, Unit, C> Ap<B, C>(
        KArr<Option, Unit, Unit, Unit, Func<B, C>> f,
        KArr<Option, Unit, Unit, Unit, B> x) =>
        new Option<C>(f.Morphism.Apply(x.Morphism));

    public static KArr<Option, Unit, Unit, Unit, C> Bind<B, C>(
        KArr<Option, Unit, Unit, Unit, B> mx,
        Transducer<B, KArr<Option, Unit, Unit, Unit, C>> f) =>
        new Option<C>(mx.Morphism.Bind(f.Map(static b => b.Morphism)));
}

public static class Test
{
    public static KArr<F, Env, int> Add<F, Env>(KArr<F, Env, int> mx, KArr<F, Env, int> my)
        where F : Apply<F, Env>, Applicative<F, Env> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);

    public static KArr<F, Env, X, Env, int> Add<F, Env, X>(KArr<F, Env, X, Env, int> mx, KArr<F, Env, X, Env, int> my)
        where F : ApplySum<F, Env, X>, ApplicativeSum<F, Env, X> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);

    public static Func<int, Func<int, int>> addF = 
        x => y => x + y; 

    public static Option<int> Test1() =>
        Add(Option<int>.Some(123), Option<int>.Some(123)).Morphism;
}
#1#
*/

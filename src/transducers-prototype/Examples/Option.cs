using LanguageExt.HKT;

namespace LanguageExt.Examples;

public readonly record struct Option<A>(SumTransducer<Unit, Unit, Unit, A> MorphismUnsafe) 
    : K<Option, Unit, Unit, Unit, A>
{
    Transducer<Sum<Unit, Unit>, Sum<Unit, A>> K<Option, Sum<Unit, Unit>, Sum<Unit, A>>.Morphism =>
        MorphismUnsafe;

    public SumTransducer<Unit, Unit, Unit, A> Morphism => 
        MorphismUnsafe;

    public static Option<A> Some(A value) =>
        SumTransducer.Right<Unit, A>(value);
    
    public static readonly Option<A> None =
        SumTransducer.Left<Unit, A>(default);

    public static Option<A> Optional(A? value) =>
        value is null 
            ? None 
            : Some(value);
    
    public static implicit operator Option<A>(SumTransducer<Unit, Unit, Unit, A> m) =>
        new (m);
    
    public static implicit operator Option<A>(Transducer<Sum<Unit, Unit>, Sum<Unit, A>> m) =>
        new (SumTransducer.lift(m));

    public Option<B> Map<B>(Func<A, B> f) =>
        Morphism.Map(f);

    public Option<B> Select<B>(Func<A, B> f) =>
        Morphism.Map(f);

    public Option<B> Bind<B>(Func<A, Option<B>> f) =>
        Morphism.Bind(x => f(x).Morphism);

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

public readonly struct Option : 
    MonadSum<Option, Unit, Unit>, 
    ApplySum<Option, Unit, Unit>
{
    public static K<Option, Unit, Unit, Unit, A> Lift<A>(SumTransducer<Unit, Unit, Unit, A> f) =>
        new Option<A>(f);

    public static K<Option, Unit, Unit, Unit, B> BiMap<A, B>(
        K<Option, Unit, Unit, Unit, A> fab, 
        Transducer<Unit, Unit> Left, 
        Transducer<A, B> Right) =>
        new Option<B>(SumTransducer.compose(fab.Morphism, SumTransducer.bimap(Left, Right)));

    public static K<Option, Unit, Unit, Unit, C> Ap<B, C>(
        K<Option, Unit, Unit, Unit, Func<B, C>> f,
        K<Option, Unit, Unit, Unit, B> x) =>
        new Option<C>(f.Morphism.Apply(x.Morphism));

    public static K<Option, Unit, Unit, Unit, C> Bind<B, C>(
        K<Option, Unit, Unit, Unit, B> mx,
        Transducer<B, K<Option, Unit, Unit, Unit, C>> f) =>
        new Option<C>(mx.Morphism.Bind(f.Map(static b => b.Morphism)));
}

public static class Test
{
    public static K<F, A, int> Add<F, A>(K<F, A, int> mx, K<F, A, int> my)
        where F : Apply<F, A>, Applicative<F, A> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);

    public static K<F, X, X, A, int> Add<F, X, A>(K<F, X, X, A, int> mx, K<F, X, X, A, int> my)
        where F : ApplySum<F, X, A>, ApplicativeSum<F, X, A> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);

    public static Func<int, Func<int, int>> addF = 
        x => y => x + y; 

    public static Option<int> Test1() =>
        Add(Option<int>.Some(123), Option<int>.Some(123)).Morphism;
}

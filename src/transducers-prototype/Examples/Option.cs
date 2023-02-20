using LanguageExt.HKT;

namespace LanguageExt.Examples;

public readonly record struct Option<A>(Transducer<Sum<Unit, Unit>, Sum<Unit, A>> MorphismUnsafe) 
    : K<Option, Sum<Unit, Unit>, Sum<Unit, A>>
{
    public Transducer<Sum<Unit, Unit>, Sum<Unit, A>> Morphism =>
        MorphismUnsafe;

    public static Option<A> Some(A value) =>
        SumTransducer.Pure(Sum<Unit, A>.Right(value));
    
    public static readonly Option<A> None =
        SumTransducer.Pure(Sum<Unit, A>.Left(default));

    public static Option<A> Optional(A? value) =>
        value is null 
            ? None 
            : Some(value);
    
    public static implicit operator Option<A>(Transducer<Sum<Unit, Unit>, Sum<Unit, A>> m) =>
        new (m);
}

public class Option : 
    MonadSum<Option, Unit, Unit>, 
    ApplySum<Option, Unit, Unit>
{
    public static K<Option, Sum<Unit, Unit>, Sum<Unit, A>> Lift<A>(Transducer<Sum<Unit, Unit>, Sum<Unit, A>> f) =>
        new Option<A>(f);

    public static K<Option, Sum<Unit, Unit>, Sum<Unit, B>> BiMap<A, B>(
        K<Option, Sum<Unit, Unit>, Sum<Unit, A>> fab, 
        Transducer<Unit, Unit> Left, 
        Transducer<A, B> Right) =>
        new Option<B>(Transducer.compose(fab.Morphism, SumTransducer.bimap(Left, Right)));

    public static K<Option, Sum<Unit, Unit>, Sum<Unit, C>> Ap<B, C>(
        K<Option, Sum<Unit, Unit>, Sum<Unit, Func<B, C>>> f,
        K<Option, Sum<Unit, Unit>, Sum<Unit, B>> x) =>
        new Option<C>(f.Morphism.Apply(x.Morphism));

    public static K<Option, Sum<Unit, Unit>, Sum<Unit, C>> Bind<B, C>(
        K<Option, Sum<Unit, Unit>, Sum<Unit, B>> mx, 
        Transducer<B, K<Option, Sum<Unit, Unit>, Sum<Unit, C>>> f)
    {
        throw new NotImplementedException();
    }
}

public static class Test
{
    public static K<F, A, int> Add<F, A>(K<F, A, int> mx, K<F, A, int> my)
        where F : Apply<F, A>, Applicative<F, A> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);

    public static K<F, Sum<X, A>, Sum<X, int>> Add<F, X, A>(K<F, Sum<X, A>, Sum<X, int>> mx, K<F, Sum<X, A>, Sum<X, int>> my)
        where F : ApplySum<F, X, A>, ApplicativeSum<F, X, A> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);

    public static Func<int, Func<int, int>> addF = 
        x => y => x + y; 

    public static Option<int> Test1() =>
        Add(Option<int>.Some(123), Option<int>.Some(123)).Morphism;
}

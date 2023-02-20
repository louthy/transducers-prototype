using LanguageExt.HKT;

namespace LanguageExt.Examples;

public readonly record struct Option<A>(Transducer<Sum<Unit, Unit>, Sum<Unit, A>> MorphismUnsafe) 
    : K<Option, Sum<Unit, Unit>, Sum<Unit, A>>
{
    public Transducer<Sum<Unit, Unit>, Sum<Unit, A>> Morphism =>
        MorphismUnsafe;

    public static Option<A> Some(A value) =>
        new(SumTransducer.Pure(Sum<Unit, A>.Right(value)));
    
    public static readonly Option<A> None =
        new(SumTransducer.Pure(Sum<Unit, A>.Left(default)));

    public static Option<A> Optional(A? value) =>
        value is null 
            ? None 
            : Some(value);
}

public class Option : 
    SumApplicative<Option, Unit, Unit>, 
    SumApply<Option, Unit, Unit>
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
}

public static class Test
{
    /*
    public static K<F, A, int> Add1<F, A>(K<F, A, int> mx, K<F, A, int> my)
        where F : Apply<F, A>, Applicative<F, A> =>
            F.Ap(F.Ap(F.Pure(addF), mx), my);
            */
    
    public static K<F, A, int> Add2<F, A>(K<F, A, int> mx, K<F, A, int> my)
        where F : Apply<F, A>, Applicative<F, A> =>
            F.Ap(F.Ap(Applicative<F, A>.Pure(addF), mx), my);

    public static Func<int, Func<int, int>> addF = 
        x => y => x + y; 
 
    /*
    public static K<F, Sum<X, A>, Sum<X, int>> Add<F, X, A>(K<F, Sum<X, A>, Sum<X, int>> mx, K<F, Sum<X, A>, Sum<X, int>> my)
        where F : SumApply<F, X, A>, SumApplicative<F, X, A> =>
        F.Ap(F.Ap(SumApplicative<F, X, A>.Pure<Func<int, Func<int, int>>>(x => y => x + y), mx), my);

    public static Option<int> Test1() =>
        new(Add2(Option<int>.Some(123), Option<int>.Some(123)));
        */
}

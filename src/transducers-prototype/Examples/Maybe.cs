using LanguageExt.HKT;

namespace LanguageExt.Examples;

/// <summary>
/// Maybe data-type
/// </summary>
public abstract record Maybe<A>
{
    public static Maybe<A> Just(A value) => new JustCase<A>(value);
    public static readonly Maybe<A> Nothing = new NothingCase<A>();
}
public record JustCase<A>(A Value): Maybe<A>;
public record NothingCase<A> : Maybe<A>;

/// <summary>
/// Maybe monad
/// </summary>
public readonly record struct MaybeT<M, X, A, B>(
        SumTransducer<X, A, SumTransducer<Unit, Unit, Unit, B>, SumTransducer<Unit, Unit, Unit, B>> MorphismUnsafe)
    : K<Option, X, X, A, B>
{
    Transducer<Sum<X, A>, Sum<X, B>> K<Option, Sum<X, A>, Sum<X, B>>.Morphism =>
        MorphismUnsafe
            .Bind(tb => tb.Bind())

    SumTransducer<X, X, A, B> K<Option, X, X, A, B>.Morphism => 
        _morphism1;
}

public readonly struct MaybeT<M> : Monad<M>
    where M : Monad<M>
{
    public static K<M, A, C> Map<A, B, C>(K<M, A, B> fab, Transducer<B, C> f)
    {
        throw new NotImplementedException();
    }

    public static K<M, A, B> Lift<A, B>(Transducer<A, B> f)
    {
        throw new NotImplementedException();
    }

    public static K<M, A, C> Bind<A, B, C>(K<M, A, B> mx, Transducer<B, K<M, A, C>> f)
    {
        throw new NotImplementedException();
    }
}
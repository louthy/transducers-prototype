using static LanguageExt.Transducer;

namespace LanguageExt.Examples;

public record Either<L, R>(EitherT<MIdentity, L, R> Transformer)
{
    public static Either<L, R> Right(R value) =>
        new (EitherT<MIdentity, L, R>.Right(value));
    
    public static Either<L, R> Left(L value) =>
        new (EitherT<MIdentity, L, R>.Left(value));

    public Either<L, B> MapRight<B>(Func<R, B> f) =>
        new (Transformer.MapRight(f));
    
    public Either<B, R> MapLeft<B>(Func<L, B> f) =>
        new (Transformer.MapLeft(f));

    public Either<X, Y> BiMap<X, Y>(Func<L, X> Left, Func<R, Y> Right) =>
        new(Transformer.BiMap(Left, Right));

    public Either<L, B> Select<B>(Func<R, B> f) =>
        new (Transformer.MapRight(f));

    public Either<L, B> Bind<B>(Func<R, Either<L, B>> f) =>
        new(new EitherT<MIdentity, L, B>(Transformer.MapRight(x => f(x).Transformer.Morphism).Morphism.FlattenT()));

    public Either<L, C> SelectMany<B, C>(Func<R, Either<L, B>> bind, Func<R, B, C> f) =>
        Bind(x => bind(x).MapRight(y => f(x, y)));

    public A Match<A>(Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom) =>
        Transformer.Match(Left, Right, Bottom);
}

public static class EitherTests
{
    public static void Foo()
    {
        var mx = Either<string, int>.Left("hello");
        var my = Either<string, int>.Right(100);
        var mz = Either<string, int>.Right(200);
        
        /*var mr = from x in mx 
                 from y in my
                 select x + y;*/

    }
}
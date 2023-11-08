#nullable enable
using static LanguageExt.Transducer;
using LanguageExt.HKT;

namespace LanguageExt.Examples;

public struct MEither<M, L> : Monad<MEither<M, L>>
{
    public static KArr<MEither<M, L>, Unit, B> Map<A, B>(KArr<MEither<M, L>, Unit, A> fab, Transducer<A, B> f) =>
        throw new NotImplementedException();
    
    public static KArr<MEither<M, L>, Unit, A> Lift<A>(Transducer<Unit, A> f) =>
        throw new NotImplementedException();
    
    public static KArr<MEither<M, L>, Unit, B> Bind<A, B>(KArr<MEither<M, L>, Unit, A> mx, Transducer<A, KArr<MEither<M, L>, Unit, B>> f) =>
        throw new NotImplementedException();
}

public readonly struct Either<L, R> : KArr<Either<L, R>, Unit, Sum<L, R>>
{
    readonly EitherT<MIdentity, L, R> transformer;

    Either(EitherT<MIdentity, L, R> transformer) =>
        this.transformer = transformer;
    
    public static Either<L, R> Right(R value) =>
        new (EitherT<MIdentity, L, R>.Right(value));
    
    public static Either<L, R> Left(L value) =>
        new (EitherT<MIdentity, L, R>.Left(value));

    public Either<L, B> MapRight<B>(Func<R, B> f) =>
        new (transformer.MapRight(f));
    
    public Either<B, R> MapLeft<B>(Func<L, B> f) =>
        new (transformer.MapLeft(f));

    public Either<X, Y> BiMap<X, Y>(Func<L, X> Left, Func<R, Y> Right) =>
        new(transformer.BiMap(Left, Right));

    public Either<L, B> Select<B>(Func<R, B> f) =>
        new (transformer.MapRight(f));

    public Either<L, B> Bind<B>(Func<R, Either<L, B>> f) =>
        MapRight(f).Flatten();

    public Either<L, C> SelectMany<B, C>(Func<R, Either<L, B>> bind, Func<R, B, C> f) =>
        Bind(x => bind(x).MapRight(y => f(x, y)));

    public A Match<A>(Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom) =>
        transformer.Match(Left, Right, Bottom);

    public Transducer<Unit, Sum<L, R>> Morphism =>
        transformer.Morphism ?? throw new Exception("Either struct not initialised");
    
    public Reducer<S, Unit> Transform<S>(Reducer<S, Sum<L, R>> reduce) =>
        Morphism.Transform(reduce);
}

public static class Either
{
    public static Either<L, R> Flatten<L, R>(this Either<L, Either<L, R>> mma) =>
        mma.Bind(static mx => mx);
}
public static class EitherTests
{
    public static void Foo()
    {
        var mx = Either<string, int>.Left("hello");
        var my = Either<string, int>.Right(100);
        var mz = Either<string, int>.Right(200);
        
        var mr = from x in mx 
                 from y in my
                 select x + y;
    }
}
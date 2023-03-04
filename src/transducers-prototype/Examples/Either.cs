using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Either<L, R>(SumTransducer<Unit, L, Unit, SumTransducer<Unit, L, Unit, R>> MorphismValue) : 
    EitherT<Identity<Unit, L>, Unit, L, R>(MorphismValue)
{
}

public record Eff<Env, A>(SumTransducer<Env, Error, Env, SumTransducer<Unit, Error, Unit, A>> MorphismValue) : 
    EitherT<IO<Env, Error>, Env, Error, A>(MorphismValue)
{}

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
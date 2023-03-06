using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Either<L, R>(Transducer<Unit, SumTransducer<Unit, L, Unit, R>> MorphismValue) : 
    EitherT<Identity<Unit>, Unit, L, R>(MorphismValue)
{
}

public record Eff<Env, A>(Transducer<Env, SumTransducer<Unit, Error, Unit, A>> MorphismValue) : 
    EitherT<IO<Env>, Env, Error, A>(MorphismValue)
{}

public record Aff<Env, A>(Transducer<Env, SumTransducer<Unit, Error, Unit, A>> MorphismValue) : 
    EitherT<Async<Env>, Env, Error, A>(MorphismValue)
{
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
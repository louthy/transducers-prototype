using LanguageExt;
using static LanguageExt.Transducer;

var mx = SumTransducer.Right<Unit, int>(100); 
var my = SumTransducer.Right<Unit, int>(299);
var mf = SumTransducer.Left<Unit, int>(default);
var mz = from x in mx
         from y in my
         from _ in mf
         select x + y;


var mr = mz.Invoke1(Sum<Unit, Unit>.Right(default));

// Start up
single(0).Invoke1(default);

// Start up
loop1(0).Invoke1(default);

static Transducer<Unit, Unit> single(int x) =>
    from r in show(x)
    select r;

// Infinite loop (with space leak)
static Transducer<Unit, Unit> loop(int x) =>
    from _ in show(x)
    from r in loop(x + 1)
    select r;

// Infinite loop (no space leak)
static Transducer<Unit, Unit> loop0(int x) =>
    (from r in show(x)
     select x + 1)
    .Bind(loop0);

// Infinite loop (no space leak)
static Transducer<Unit, Unit> loop1(int x) =>
    show(x)
        .SelectMany(_ => loop1(x + 1));

// Infinite loop (with space leak)
static Transducer<Unit, Unit> loop2(int x) =>
    show(x)
        .SelectMany(_ => loop2(x + 1), static (_, r) => r);

static Transducer<Unit, Unit> show(int x) =>
    x % 100000 == 0 ? writeLine($"{x:n0}") : Pure<Unit>(default);

// Wrap up Console.WriteLine into a Transducer
static Transducer<Unit, Unit> writeLine(string x) =>
    lift(() =>
    {
        Console.WriteLine(x);
        return default(Unit);
    });

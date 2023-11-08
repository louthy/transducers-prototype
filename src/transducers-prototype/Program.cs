using LanguageExt;
using LanguageExt.Examples;
using static LanguageExt.Transducer;

var result1 = AppTest.Example1.Invoke1("Paul");
Console.WriteLine(result1);

var result2 = AppTest.Example2.Invoke1("Paul");
Console.WriteLine(result2);

AsyncTest.Main();
return;

/*
var ax = Aff<Unit, int>.Right(100);
var ay = Aff<Unit, int>.Right(200);
var az =
    from x in ax
    from y in ay
    select x + y;

//var ar = az.In

//Func<string, int> foo = x => x.Length;
//var task = foo.BeginInvoke("Hello", null, null);
//var res = foo.EndInvoke(task);

// ---------------------------------------------------------------------------------------------------------------------
// Transducer bind tests
var mx = SumTransducer.Right<Unit, int>(100); 
var my = SumTransducer.Right<Unit, int>(299);
var mf = SumTransducer.Left<Unit, int>(default);
var mz = from b in lift<Unit, int>(_ => 100)
         from x in mx
         from y in my
         from _ in mf
         select x + y;

var mr = mz.Invoke1(Sum<Unit, Unit>.Right(default));

// ---------------------------------------------------------------------------------------------------------------------
// Option bind tests
var ox = Option<int>.Some(100); 
var oy = Option<int>.Some(299);
var of = Option<int>.None;

var oz = from x in ox
         from y in oy
         from _ in of
         select x + y;

var or = oz.Match(Some: x => x, None: () => -1);

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
*/
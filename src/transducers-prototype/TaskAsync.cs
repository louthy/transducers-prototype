using System.Diagnostics;

namespace LanguageExt;

public static class AsyncTest
{
    public static void Run()
    {
        var sw = Stopwatch.StartNew(); 
        var result = TaskAsync<string>.Run(Long, default);
        var ts = sw.Elapsed;
        Console.WriteLine($"{result} in {ts}");
    }

    static async Task<string> Long(CancellationToken token)
    {
        await Task.Delay(2000);
        //throw new Exception("blah");
        await Task.Delay(3000);
        return "Hello";
    }
}

public static class TaskAsync<A>
{
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<A> Run(Func<CancellationToken, Task<A>> f, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<A>();

        // Launch the task
        var t = f(token);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!t.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (t.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<A>();
        }

        if (t.IsCompletedSuccessfully)
        {
            return TResult.Continue(t.Result);
        }

        if (t.IsFaulted)
        {
            return t.Exception is null
                ? TResult.None<A>()
                : TResult.Fail<A>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<A> Run(Func<CancellationToken, ValueTask<A>> f, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<A>();

        // Launch the task
        var vt = f(token);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!vt.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (vt.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<A>();
        }

        if (vt.IsCompletedSuccessfully)
        {
            return TResult.Continue(vt.Result);
        }

        if (vt.IsFaulted)
        {
            var t = vt.AsTask();
            return t.Exception is null
                ? TResult.None<A>()
                : TResult.Fail<A>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static Result<A> Run(Func<A> f, CancellationToken token)
    {
        if (token.IsCancellationRequested) return Result<A>.Cancelled;

        // Launch the task
        var t = Task.Run(f, token);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!t.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (t.IsCanceled || token.IsCancellationRequested)
        {
            return Result<A>.Cancelled;
        }

        if (t.IsCompletedSuccessfully)
        {
            return Result.Value(t.Result);
        }

        if (t.IsFaulted)
        {
            return Result.Fail<A>(
                t.Exception is null
                    ? Errors.None
                    : Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<A> Run(Func<CancellationToken, Task<TResult<A>>> f, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<A>();

        // Launch the task
        var t = f(token);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!t.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (t.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<A>();
        }

        if (t.IsCompletedSuccessfully)
        {
            return t.Result;
        }

        if (t.IsFaulted)
        {
            return t.Exception is null
                ? TResult.None<A>()
                : TResult.Fail<A>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<A> Run(Func<CancellationToken, ValueTask<TResult<A>>> f, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<A>();

        // Launch the task
        var vt = f(token);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!vt.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (vt.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<A>();
        }

        if (vt.IsCompletedSuccessfully)
        {
            return vt.Result;
        }

        if (vt.IsFaulted)
        {
            var t = vt.AsTask();
            return t.Exception is null
                    ? TResult.None<A>()
                    : TResult.Fail<A>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="value">Value to pass to the function to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<B> Run<B>(Func<CancellationToken, A, Task<TResult<B>>> f, A value, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<B>();

        // Launch the task
        var t = f(token, value);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!t.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (t.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<B>();
        }

        if (t.IsCompletedSuccessfully)
        {
            return t.Result;
        }

        if (t.IsFaulted)
        {
            return t.Exception is null
                    ? TResult.None<B>()
                    : TResult.Fail<B>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="value">Value to pass to the function to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<B> Run<B>(Func<CancellationToken, A, ValueTask<TResult<B>>> f, A value, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<B>();

        // Launch the task
        var vt = f(token, value);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!vt.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (vt.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<B>();
        }

        if (vt.IsCompletedSuccessfully)
        {
            return vt.Result;
        }

        if (vt.IsFaulted)
        {
            var t = vt.AsTask();
            return t.Exception is null
                ? TResult.None<B>()
                : TResult.Fail<B>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
        
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="value">Value to pass to the function to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<B> Run<B>(Func<CancellationToken, A, Task<B>> f, A value, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<B>();

        // Launch the task
        var t = f(token, value);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!t.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (t.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<B>();
        }

        if (t.IsCompletedSuccessfully)
        {
            return TResult.Continue(t.Result);
        }

        if (t.IsFaulted)
        {
            return t.Exception is null
                    ? TResult.None<B>()
                    : TResult.Fail<B>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
    
    /// <summary>
    /// Runs a task concurrently and yields whilst waiting
    /// </summary>
    /// <param name="f">Function that yields a task to run</param>
    /// <param name="value">Value to pass to the function to run</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    public static TResult<B> Run<B>(Func<CancellationToken, A, ValueTask<B>> f, A value, CancellationToken token)
    {
        if (token.IsCancellationRequested) return TResult.Cancel<B>();

        // Launch the task
        var vt = f(token, value);

        // Spin waiting for the task to complete or be cancelled
        SpinWait sw = default;
        while (!vt.IsCompleted && !token.IsCancellationRequested)
        {
            sw.SpinOnce();
        }

        if (vt.IsCanceled || token.IsCancellationRequested)
        {
            return TResult.Cancel<B>();
        }

        if (vt.IsCompletedSuccessfully)
        {
            return TResult.Continue(vt.Result);
        }

        if (vt.IsFaulted)
        {
            var t = vt.AsTask();
            return t.Exception is null
                ? TResult.None<B>()
                : TResult.Fail<B>(Error.New(t.Exception.InnerExceptions.FirstOrDefault() ?? t.Exception));
        }

        throw new UnreachableException();
    }
}

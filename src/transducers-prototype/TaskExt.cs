namespace LanguageExt;

public static class TaskExt
{
    public static async Task<B> Map<A, B>(this Task<A> ta, Func<A, B> f) =>
        f(await ta);
    
    public static async ValueTask<B> Map<A, B>(this ValueTask<A> ta, Func<A, B> f) =>
        f(await ta);
}
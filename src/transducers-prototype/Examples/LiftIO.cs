/*namespace LanguageExt.Examples;

public struct Lifted<A> : Transducer<CancellationToken, A>
{
    readonly Transducer<CancellationToken, Task<A>> morphism;

    public Lifted(Transducer<CancellationToken, Task<A>> morphism) =>
        this.morphism = morphism;

    public Transducer<CancellationToken, A> Morphism =>
        this;
        
    public Reducer<S, CancellationToken> Transform<S>(Reducer<S, A> reduce)
    {
        
    }
}*/
namespace LanguageExt.HKT;


/*
public interface KArr<M, E, A>
{
    Transducer<E, A> Morphism { get; }
}

public interface KArr<M, A> : KArr<M, Unit, A>
{
}

public interface Monoid<A> where A : Monoid<A>
{
    public static abstract A Empty();
    public static abstract A operator+(A lhs, A rhs);
}

public interface Foldable<out A>
{
    S Fold<S>(S state, Func<S, A, S> f);
    
    public M FoldMap<M>(Func<A, M> f) where M : Monoid<M> =>
        Fold(M.Empty(), (s, x) => s + f(x));
    
    //   fold    :: Monoid m => t m -> m
    //   foldMap :: Monoid m => (a -> m) -> t a -> m
    //   foldr   :: (a -> b -> b) -> b -> t a -> b
    //   foldr'  :: (a -> b -> b) -> b -> t a -> b
    //   foldl   :: (b -> a -> b) -> b -> t a -> b
    //   foldl'  :: (b -> a -> b) -> b -> t a -> b
    //   foldr1  :: (a -> a -> a) -> t a -> a
    //   foldl1  :: (a -> a -> a) -> t a -> a
    //   toList  :: t a -> [a]
    //   null    :: t a -> Bool
    //   length  :: t a -> Int
    //   elem    :: Eq a => a -> t a -> Bool
    //   maximum :: Ord a => t a -> a
    //   minimum :: Ord a => t a -> a
    //   sum     :: Num a => t a -> a
    //   product :: Num a => t a -> a
}

/*
public interface Functor<F, E>
{
    public static abstract KArr<F, E, B> Map<A, B>(KArr<F, E, A> fa, Func<A, B> f); 
    public static abstract KArr<F, E, B> Map<A, B>(KArr<F, E, A> fa, Transducer<A, B> f); 
}
#1#

public interface FunctorE<F>
{
    public static abstract KArr<F, E, B> Map<E, A, B>(KArr<F, E, A> fa, Func<A, B> f); 
    public static abstract KArr<F, E, B> Map<E, A, B>(KArr<F, E, A> fa, Transducer<A, B> f); 
}

public interface Functor<F>
{
    public static abstract KArr<F, B> Map<A, B>(KArr<F, A> fa, Func<A, B> f); 
    public static abstract KArr<F, B> Map<A, B>(KArr<F, A> fa, Transducer<A, B> f); 
}

public interface ApplicativeE<F> : FunctorE<F>
{
    public static abstract KArr<F, E, A> Pure<E, A>(A value); 
    public static abstract KArr<F, E, B> Apply<E, A, B>(KArr<F, E, Func<A, B>> ff, KArr<F, E, A> fa);
}

public interface Applicative<F> : Functor<F>
{
    public static abstract KArr<F, A> Pure<A>(A value); 
    public static abstract KArr<F, B> Apply<A, B>(KArr<F, Func<A, B>> ff, KArr<F, A> fa);
}

public interface MonadReader<F> : ApplicativeE<F>
{
    public static abstract KArr<F, E, A> Lift<E, A>(Transducer<E, A> f);
    public static abstract KArr<F, E, B> Bind<E, A, B>(KArr<F, E, A> ma, Func<A, KArr<F, E, B>> f);
}

public interface Monad<F> : Applicative<F>
{
    public static abstract KArr<F, A> Lift<A>(Transducer<Unit, A> f);
    public static abstract KArr<F, B> Bind<A, B>(KArr<F, A> ma, Func<A, KArr<F, B>> f);
}
*/

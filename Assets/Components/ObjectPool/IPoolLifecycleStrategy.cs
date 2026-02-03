namespace Components.ObjectPool
{
    public interface IPoolLifecycleStrategy
    {
        void OnGet(PoolObject obj);
        void OnRelease(PoolObject obj);
    }
}

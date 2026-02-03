namespace Components.ObjectPool
{
    public interface IObjectPoolManager
    {
        bool CreatePool(PoolObject objectToPool, IPoolLifecycleStrategy lifecycleStrategy, int maxSize = 50, int defaultCapacity = 10, bool collectionCheck = true);
        bool Spawn(PoolObject poolObject, out PoolObject poolObjectInstance);
        bool Release(PoolObject poolObjectInstance);
    }
}

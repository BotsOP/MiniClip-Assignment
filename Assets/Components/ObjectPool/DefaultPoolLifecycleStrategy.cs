namespace Components.ObjectPool
{
    public class DefaultPoolLifecycleStrategy : IPoolLifecycleStrategy
    {
        public void OnGet(PoolObject obj)
        {
            obj.gameObject.SetActive(true);
        }

        public void OnRelease(PoolObject obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}

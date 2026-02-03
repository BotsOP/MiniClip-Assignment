using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Components.ObjectPool
{
    public class ObjectPoolManager : MonoBehaviour, IDependencyProvider, IObjectPoolManager
    {
        private Dictionary<Type, ObjectPool<PoolObject>> pools = new Dictionary<Type, ObjectPool<PoolObject>>();

        [Provide] private IObjectPoolManager ProvideObjectPoolManager()
        {
            return this;
        }
        
        public bool CreatePool(PoolObject objectToPool, IPoolLifecycleStrategy lifecycleStrategy, int maxSize = 50, int defaultCapacity = 10, bool collectionCheck = true)
        {
            if (pools.ContainsKey(objectToPool.GetType()))
            {
                // Debug.Log($"Object of type {objectToPool.GetType()} has an active object pool");
                return false;
            }

            lifecycleStrategy ??= new DefaultPoolLifecycleStrategy();
            
            ObjectPool<PoolObject> newPool = new ObjectPool<PoolObject>(
                createFunc: () =>
                {
                    PoolObject poolObjectIntance = Instantiate(objectToPool);
                    poolObjectIntance.gameObject.SetActive(false);
                    return poolObjectIntance;
                },
                actionOnGet: lifecycleStrategy.OnGet,
                actionOnRelease: lifecycleStrategy.OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: collectionCheck,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
            
            pools.Add(objectToPool.GetType(), newPool);
            return true;
        }

        public bool Spawn(PoolObject poolObject, out PoolObject poolObjectInstance)
        {
            poolObjectInstance = null;
            if (!pools.ContainsKey(poolObject.GetType()))
            {
                Debug.LogError($"No exisiting object pool of type {poolObject.GetType()}. Make sure to instantiate an object pool first");
                return false;
            }

            poolObjectInstance = pools[poolObject.GetType()].Get();
            return true;
        }

        public bool Release(PoolObject poolObjectInstance)
        {
            if (!pools.ContainsKey(poolObjectInstance.GetType()))
            {
                Debug.LogError($"No exisiting object pool of type {poolObjectInstance.GetType()}. Make sure to instantiate an object pool first");
                return false;
            }
            
            pools[poolObjectInstance.GetType()].Release(poolObjectInstance);
            return true;
        }
        
        private void OnDestroyItem(PoolObject poolObject)
        {
            Destroy(poolObject.gameObject);
        }
    }
}

using Components.ObjectPool;
using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    public abstract class MoleViewer : PoolObject
    {
        [SerializeField] private Animator animator;
        public virtual Animator GetAnimator()
        {
            return animator;
        }
        public virtual PoolObject GetPoolObject()
        {
            return this;
        }
    }

}

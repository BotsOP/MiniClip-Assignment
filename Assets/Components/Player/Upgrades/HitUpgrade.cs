using Components.Grid;
using Components.ObjectPool;
using Managers;
using PrimeTween;
using UnityEngine;
using EventType = Managers.EventType;

namespace Components.Player.Upgrades
{
    public abstract class HitUpgrade : IHitResolver
    {
        protected readonly int level;
        protected readonly GridContext gridContext;
        protected readonly IHitResolver inner;

        protected HitUpgrade(IHitResolver inner, GridContext gridContext, int level)
        {
            this.inner = inner;
            this.gridContext = gridContext;
            this.level = level;
        }

        protected virtual void ExtraHit(HammerData hammerData, DamageInfo damageInfo)
        {
            if (!hammerData.spawn(hammerData.extraHammerInstance, out PoolObject instance))
            {
                Debug.LogError($"Couldn't spawn extra hammer instance for {GetType()} upgrade");
                return;
            }
            
            var release = hammerData.release;
            instance.transform.position = damageInfo.worldPos;
            instance.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 90);
            Sequence.Create(1, CycleMode.Restart, Ease.OutSine)
                .Group(Tween.LocalRotation(instance.transform, Quaternion.identity, 0.1f))
                .ChainCallback(() => release(instance));
            EventSystem<DamageInfo>.RaiseEvent(EventType.DoDamage, damageInfo);
        }
        
        public virtual void ResolveHit(ref HammerData hammerData) {}
    }
}

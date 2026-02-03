using Components.Grid;
using Components.Managers;
using Components.ObjectPool;
using PrimeTween;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Player.Upgrades
{
    public abstract class HitUpgrade : IHitResolver
    {
        protected int Level { get; private set; }
        protected readonly IHitResolver inner;
        protected GridContext gridContext;
        protected IDamageManager damageManager;

        protected HitUpgrade(IHitResolver inner, IDamageManager damageManager, GridContext gridContext, int level)
        {
            this.inner = inner;
            this.damageManager = damageManager;
            this.gridContext = gridContext;
            Level = level;
        }

        protected virtual void ExtraHit(HammerData hammerData, DamageInfo damageInfo)
        {
            if (!hammerData.spawn(hammerData.extraHammerInstance, out PoolObject instance))
            {
                Debug.LogError($"Couldn't spawn extra hammer instance for {GetType()} upgrade");
                return;
            }
            
            instance.transform.position = damageInfo.worldPos + new Vector3(damageInfo.offset.x * gridContext.cellSize, 0, damageInfo.offset.y * gridContext.cellSize);
            instance.transform.rotation = hammerData.baseHammerRotation;
            instance.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 90);
            Sequence.Create(1, CycleMode.Restart, Ease.OutSine)
                .Group(Tween.LocalRotation(instance.transform.GetChild(0), Quaternion.identity, 0.1f))
                .Group(Tween.Delay(1))
                .ChainCallback(() => hammerData.release(instance));
            damageManager.DoDamage(damageInfo);
        }
        
        public virtual void ResolveLevelChanges(){}
        public void LevelUp()
        {
            Level++;
            ResolveLevelChanges();
        }
        
        public virtual void ResolveHit(ref HammerData hammerData) {}
    }
}

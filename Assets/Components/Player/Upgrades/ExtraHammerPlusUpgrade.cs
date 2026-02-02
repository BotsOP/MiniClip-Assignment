using System;
using Components.Grid;
using Components.ObjectPool;
using Managers;
using PrimeTween;
using UnityEngine;
using EventType = Managers.EventType;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/ExtraHammerPlusUpgrade")]
    public class ExtraHammerPlusUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
        [SerializeField] private int upgradeOrder;
        [SerializeField] private float extraHammerDamageBase = 0.25f;
        [SerializeField] private float extraHammerDamagePerLevel = 0.25f;
        private int level;

        public int GetUpgradeOrder()
        {
            return upgradeOrder;
        }
        public void IncreaseLevel()
        {
            level++;
        }
        
        public IHitResolver Create(IHitResolver inner, GridContext gridContext)
        {
            return new ExtraHammerPlusUpgrade(
                inner,
                gridContext,
                level,
                extraHammerDamagePerLevel,
                extraHammerDamageBase
            );
        }
    }
    
    public class ExtraHammerPlusUpgrade : HitUpgrade
    {
        private readonly float extraHammerDamagePerLevel;
        private readonly float extraHammerDamageBase;
        public ExtraHammerPlusUpgrade(IHitResolver inner, GridContext gridContext, int level, float extraHammerDamagePerLevel, float extraHammerDamageBase) : base(inner, gridContext, level)
        {
            this.extraHammerDamagePerLevel = extraHammerDamagePerLevel;
            this.extraHammerDamageBase = extraHammerDamageBase;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            float damage = hammerData.damage * (extraHammerDamageBase + extraHammerDamagePerLevel * level);
            for (int i = 0; i < level; i++)
            {
                int extension = i / 4;
                int direction = i % 4;
                DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, damage);
                Vector2Int offset = Vector2Int.zero;
                switch (direction)
                {
                    case 0 :
                        offset = new Vector2Int(0, 1 * extension);
                        break;
                    case 1 :
                        offset = new Vector2Int(1 * extension, 0);
                        break;
                    case 2 :
                        offset = new Vector2Int(0, -1 * extension);
                        break;
                    case 3 :
                        offset = new Vector2Int(-1 * extension, 0);
                        break;
                    default:
                        Debug.LogError($"Direction not found");
                        break;
                }
                damageInfo.worldPos += new Vector3(offset.x * gridContext.cellSize, 0, offset.y * gridContext.cellSize);
                
                ExtraHit(hammerData, damageInfo);
            }
            inner.ResolveHit(ref hammerData);
        }
    }
}

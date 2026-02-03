using System;
using Components.Grid;
using Components.ObjectPool;
using PrimeTween;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/ExtraHammerPlusUpgrade")]
    public class ExtraHammerPlusUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
        [Header("Visual Settings")] 
        [SerializeField] private string nameUpgrade;
        [SerializeField] private Texture2D textureUpgrade;
        [SerializeField] private string nameExtraHammerDamageModifier;
        [SerializeField] private string nameAmountExtraHammerModifier;
        
        [Header("Upgrade Settings")]
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
        public void ResetLevel()
        {
            level = 0;
        }
        
        public IHitResolver Create(IHitResolver inner, GridContext gridContext, IDamageManager damageManager)
        {
            return new ExtraHammerPlusUpgrade(
                inner,
                damageManager,
                gridContext,
                level,
                extraHammerDamagePerLevel,
                extraHammerDamageBase
            );
        }
        
        public UpgradeInfo GetUpgradeInfo()
        {
            return new UpgradeInfo(level, nameUpgrade, textureUpgrade, GetUpgradeStatInfo());
        }

        private UpgradeStatInfo[] GetUpgradeStatInfo()
        {
            UpgradeStatInfo[] upgradeInfos = new UpgradeStatInfo[2];
            ExtraHammerPlusUpgrade upgrade = new ExtraHammerPlusUpgrade(
                null,
                null,
                default,
                level,
                extraHammerDamagePerLevel,
                extraHammerDamageBase
            );
            
            UpgradeStatInfo explosionDamageStatInfo = new UpgradeStatInfo {
                nameStatModified = nameAmountExtraHammerModifier,
                valueBefore = level,
            };
            UpgradeStatInfo explosionDistanceStatInfo = new UpgradeStatInfo {
                nameStatModified = nameExtraHammerDamageModifier,
                valueBefore = upgrade.ExtraHammerDamage,
            };
            upgrade.LevelUp();
            explosionDamageStatInfo.valueAfter = level + 1;
            explosionDistanceStatInfo.valueAfter = upgrade.ExtraHammerDamage;

            upgradeInfos[0] = explosionDamageStatInfo;
            upgradeInfos[1] = explosionDistanceStatInfo;
            return upgradeInfos;
        }
    }
    
    public class ExtraHammerPlusUpgrade : HitUpgrade
    {
        private readonly float extraHammerDamagePerLevel;
        public float ExtraHammerDamage { get; private set; }
        public ExtraHammerPlusUpgrade(IHitResolver inner, IDamageManager damageManager, GridContext gridContext, int level, float extraHammerDamagePerLevel, float extraHammerDamageBase) : base(inner, damageManager, gridContext, level)
        {
            this.extraHammerDamagePerLevel = extraHammerDamagePerLevel;
            ExtraHammerDamage = extraHammerDamageBase;
            ResolveLevelChanges();
        }
        public override void ResolveLevelChanges()
        {
            ExtraHammerDamage += extraHammerDamagePerLevel * Level;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            float damage = hammerData.damage * ExtraHammerDamage;
            for (int i = 0; i < Level; i++)
            {
                int extension = (i / 4) + 1;
                int direction = i % 4;
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
                DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, offset, damage, DamageSource.Player);
                ExtraHit(hammerData, damageInfo);
            }
            inner.ResolveHit(ref hammerData);
        }
    }
}

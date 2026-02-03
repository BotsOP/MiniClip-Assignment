using Components.Grid;
using UnityEngine;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/DamageUpgrade")]
    public class DamageUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
        [Header("Visual Settings")] 
        [SerializeField] private string nameUpgrade;
        [SerializeField] private Texture2D textureUpgrade;
        [SerializeField] private string nameDamageModifier;
        
        [Header("Upgrade Settings")]
        [SerializeField] private int upgradeOrder;
        [SerializeField] private float damageIncreasePerLevel = 0.25f;
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
            return new DamageUpgrade(
                inner,
                damageManager,
                gridContext,
                level,
                damageIncreasePerLevel
            );
        }
        
        public UpgradeInfo GetUpgradeInfo()
        {
            return new UpgradeInfo(level, nameUpgrade, textureUpgrade, GetUpgradeStatInfo());
        }

        private UpgradeStatInfo[] GetUpgradeStatInfo()
        {
            UpgradeStatInfo[] upgradeInfos = new UpgradeStatInfo[1];
            DamageUpgrade upgrade = new DamageUpgrade(
                null,
                null,
                default,
                level,
                damageIncreasePerLevel
            );
            
            UpgradeStatInfo explosionDamageStatInfo = new UpgradeStatInfo {
                nameStatModified = nameDamageModifier,
                valueBefore = upgrade.damageIncreasePerLevel,
            };

            upgrade.LevelUp();
            explosionDamageStatInfo.valueAfter = upgrade.damageIncreasePerLevel;

            upgradeInfos[0] = explosionDamageStatInfo;
            return upgradeInfos;
        }
    }
    
    public class DamageUpgrade : HitUpgrade
    {
        public float damageIncreasePerLevel { get; private set; }
        
        public DamageUpgrade(IHitResolver inner, IDamageManager damageManager, GridContext gridContext, int level, float damageIncreasePerLevel) : base(inner, damageManager, gridContext, level)
        {
            this.damageIncreasePerLevel = damageIncreasePerLevel;
        }
        
        public override void ResolveLevelChanges()
        {
            //We do Level + 1 because you start with level 0 and a value to the power of 0 is always 1, and we want an immediate effect in our damage
            damageIncreasePerLevel = Mathf.Pow(damageIncreasePerLevel, Level + 1);
        }
        
        public override void ResolveHit(ref HammerData hammerData)
        {
            hammerData.damage *= damageIncreasePerLevel;
            inner.ResolveHit(ref hammerData);
        }
    }

}

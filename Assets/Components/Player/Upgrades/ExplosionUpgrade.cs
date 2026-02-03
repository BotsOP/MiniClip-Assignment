using Components.Grid;
using UnityEngine;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/ExplosionUpgrade")]
    public class ExplosionUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
        [Header("Visual Settings")] 
        [SerializeField] private string nameUpgrade;
        [SerializeField] private Texture2D textureUpgrade;
        [SerializeField] private string nameExplosionDamageModifier;
        [SerializeField] private string nameExplosionDistanceModifier;
        
        [Header("Upgrade Settings")]
        [SerializeField] private int upgradeOrder;
        [SerializeField] private float startExplosionDamageModifier = 0.25f;
        [SerializeField] private float explosionDamageModifierPerLevel = 0.01f;
        [SerializeField] private float explosionReachIncreasePerLevel = 0.25f;
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
            return new ExplosionUpgrade(
                inner,
                damageManager,
                gridContext,
                level,
                startExplosionDamageModifier,
                explosionDamageModifierPerLevel,
                explosionReachIncreasePerLevel
            );
        }

        public UpgradeInfo GetUpgradeInfo()
        {
            return new UpgradeInfo(level, nameUpgrade, textureUpgrade, GetUpgradeStatInfo());
        }

        private UpgradeStatInfo[] GetUpgradeStatInfo()
        {
            UpgradeStatInfo[] upgradeInfos = new UpgradeStatInfo[2];
            ExplosionUpgrade upgrade = new ExplosionUpgrade(
                null,
                null,
                default,
                level,
                startExplosionDamageModifier,
                explosionDamageModifierPerLevel,
                explosionReachIncreasePerLevel
            );
            
            UpgradeStatInfo explosionDamageStatInfo = new UpgradeStatInfo {
                nameStatModified = nameExplosionDamageModifier,
                valueBefore = upgrade.explosionDamageModifier,
            };
            UpgradeStatInfo explosionDistanceStatInfo = new UpgradeStatInfo {
                nameStatModified = nameExplosionDamageModifier,
                valueBefore = upgrade.explosionReachIncreasePerLevel,
            };
            upgrade.LevelUp();
            explosionDamageStatInfo.valueAfter = upgrade.explosionDamageModifier;
            explosionDistanceStatInfo.valueAfter = upgrade.explosionReachIncreasePerLevel;

            upgradeInfos[0] = explosionDamageStatInfo;
            upgradeInfos[1] = explosionDistanceStatInfo;
            return upgradeInfos;
        }
    }
    
    public sealed class ExplosionUpgrade : HitUpgrade
    {
        public float explosionDamageModifier { get; private set; }
        public float explosionDamageModifierPerLevel { get; private set; }
        public float explosionReachIncreasePerLevel { get; private set; }
        public ExplosionUpgrade(IHitResolver inner, IDamageManager damageManager, GridContext gridContext, int level, float startExplosionDamageModifier, float explosionDamageModifierPerLevel, float explosionReachIncreasePerLevel) : base(inner, damageManager, gridContext, level)
        {
            explosionDamageModifier = startExplosionDamageModifier;
            this.explosionDamageModifierPerLevel = explosionDamageModifierPerLevel;
            this.explosionReachIncreasePerLevel = explosionReachIncreasePerLevel;
            ResolveLevelChanges();
        }

        public override void ResolveLevelChanges()
        {
            explosionDamageModifier += explosionDamageModifierPerLevel * Level;
            explosionReachIncreasePerLevel = 1 + explosionReachIncreasePerLevel * Level;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            float explosionDamage = hammerData.damage * explosionDamageModifier;
            int explosionDistanceInt = Mathf.FloorToInt(explosionReachIncreasePerLevel);
            for (int x = -explosionDistanceInt; x < explosionDistanceInt; x++)
            for (int y = -explosionDistanceInt; y < explosionDistanceInt; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                
                Vector2Int index = new Vector2Int(x, y);
                
                float distance = Vector2Int.Distance(index, Vector2Int.zero);
                if (distance > explosionReachIncreasePerLevel)
                    continue;

                float distanceScaled = 1 - (distance / explosionReachIncreasePerLevel);
                DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, new Vector2Int(x, y), explosionDamage * distanceScaled, DamageSource.Player);
                ExtraHit(hammerData, damageInfo);
            }
            inner.ResolveHit(ref hammerData);
        }
    }
}

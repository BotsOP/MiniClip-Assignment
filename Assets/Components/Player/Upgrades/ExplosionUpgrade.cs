using UnityEngine;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/ExplosionUpgrade")]
    public class ExplosionUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
        [SerializeField] private int upgradeOrder;
        [SerializeField] private float startExplosionDamageModifier = 0.25f;
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
        public IHitResolver Create(IHitResolver inner)
        {
            return new ExplosionUpgrade(
                inner,
                level,
                startExplosionDamageModifier,
                explosionReachIncreasePerLevel
            );
        }
    }
    
    public class ExplosionUpgrade : HitUpgrade
    {
        private readonly float startExplosionDamageModifier;
        private readonly float explosionReachIncreasePerLevel;
        public ExplosionUpgrade(IHitResolver inner, int level, float startExplosionDamageModifier, float explosionReachIncreasePerLevel) : base(inner, level)
        {
            this.startExplosionDamageModifier = startExplosionDamageModifier;
            this.explosionReachIncreasePerLevel = explosionReachIncreasePerLevel;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            float explosionDamage = hammerData.damage * startExplosionDamageModifier;
            float explosionDistance = 1 + explosionReachIncreasePerLevel * Level;
            int explosionDistanceInt = Mathf.FloorToInt(explosionDistance);
            for (int x = -explosionDistanceInt; x < explosionDistanceInt; x++)
            for (int y = -explosionDistanceInt; y < explosionDistanceInt; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                
                Vector2Int index = new Vector2Int(x, y);
                
                float distance = Vector2Int.Distance(index, Vector2Int.zero);
                if (distance > explosionDistance)
                    continue;

                float distanceScaled = 1 - (distance / explosionDistance);
                DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, explosionDamage * distanceScaled, index);
                ExtraHit(damageInfo);
            }
            inner.ResolveHit(ref hammerData);
        }
    }
}

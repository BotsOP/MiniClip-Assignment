using UnityEngine;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/ExplosionUpgrade")]
    public class ExtraHammerPlusUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
        [SerializeField] private int upgradeOrder;
        [SerializeField] private float extraHammerDamagePerLevel = 0.25f;
        [SerializeField] private float extraHammerDamageBase = 0.25f;
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
            return new ExtraHammerPlusUpgrade(
                inner,
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
        public ExtraHammerPlusUpgrade(IHitResolver inner, int level, float extraHammerDamagePerLevel, float extraHammerDamageBase) : base(inner, level)
        {
            this.extraHammerDamagePerLevel = extraHammerDamagePerLevel;
            this.extraHammerDamageBase = extraHammerDamageBase;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            float damage = hammerData.damage * (extraHammerDamageBase + extraHammerDamagePerLevel * Level);
            for (int i = 0; i < Level; i++)
            {
                int extension = i / 4;
                int direction = i % 4;
                DamageInfo damageInfo = new DamageInfo(hammerData.worldPos, damage);
                switch (direction)
                {
                    case 0 :
                        damageInfo.gridOffset = new Vector2Int(0, 1 * extension);
                        break;
                    case 1 :
                        damageInfo.gridOffset = new Vector2Int(1 * extension, 0);
                        break;
                    case 2 :
                        damageInfo.gridOffset = new Vector2Int(0, -1 * extension);
                        break;
                    case 3 :
                        damageInfo.gridOffset = new Vector2Int(-1 * extension, 0);
                        break;
                    default:
                        Debug.LogError($"Direction not found");
                        break;
                }
                ExtraHit(damageInfo);
            }
            inner.ResolveHit(ref hammerData);
        }
    }
}

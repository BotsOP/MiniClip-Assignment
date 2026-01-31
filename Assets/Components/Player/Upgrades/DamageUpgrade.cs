using UnityEngine;

namespace Components.Player.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/DamageUpgrade")]
    public class DamageUpgradeFactory : ScriptableObject, IHitUpgradeFactory
    {
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
        public IHitResolver Create(IHitResolver inner)
        {
            return new DamageUpgrade(
                inner,
                level,
                damageIncreasePerLevel
            );
        }
    }
    
    public class DamageUpgrade : HitUpgrade
    {
        private readonly float damageIncreasePerLevel;
        public DamageUpgrade(IHitResolver inner, int level, float damageIncreasePerLevel) : base(inner, level)
        {
            this.damageIncreasePerLevel = damageIncreasePerLevel;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            hammerData.damage += damageIncreasePerLevel * Level;
            inner.ResolveHit(ref hammerData);
        }
    }

}

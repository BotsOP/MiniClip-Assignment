using Components.Grid;
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
        public IHitResolver Create(IHitResolver inner, GridContext gridContext)
        {
            return new DamageUpgrade(
                inner,
                gridContext,
                level,
                damageIncreasePerLevel
            );
        }
    }
    
    public class DamageUpgrade : HitUpgrade
    {
        private readonly float damageIncreasePerLevel;
        public DamageUpgrade(IHitResolver inner, GridContext gridContext, int level, float damageIncreasePerLevel) : base(inner, gridContext, level)
        {
            this.damageIncreasePerLevel = damageIncreasePerLevel;
        }
        public override void ResolveHit(ref HammerData hammerData)
        {
            //We do Level + 1 because you start with level 0 and a value to the power of 0 is always 1, and we want an immediate effect in our damage
            hammerData.damage *= Mathf.Pow(damageIncreasePerLevel, level + 1);
            inner.ResolveHit(ref hammerData);
        }
    }

}

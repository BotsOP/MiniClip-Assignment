using System;

namespace Components.Player.Upgrades
{
    public interface IUpgradeManager
    {
        event Action<IHitResolver> UpdateHit;
        public IHitUpgradeFactory[] TryGetRandomUpgrades(int amountRandomUpgrades);
        public void AddUpgrade(IHitUpgradeFactory upgradeFactory);
    }
}

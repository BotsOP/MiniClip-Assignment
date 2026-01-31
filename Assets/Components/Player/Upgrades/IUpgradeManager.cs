using System;

namespace Components.Player.Upgrades
{
    public interface IUpgradeManager
    {
        event Action<IHitResolver> UpdateHit;
    }
}

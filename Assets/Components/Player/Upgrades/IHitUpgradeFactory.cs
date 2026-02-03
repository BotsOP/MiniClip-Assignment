using Components.Grid;

namespace Components.Player.Upgrades
{
    public interface IHitUpgradeFactory
    {
        int GetUpgradeOrder();
        void IncreaseLevel();
        void ResetLevel();
        IHitResolver Create(IHitResolver inner, GridContext gridContext, IDamageManager damageManager);
        public UpgradeInfo GetUpgradeInfo();
    }
}

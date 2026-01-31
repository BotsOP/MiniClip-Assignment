namespace Components.Player.Upgrades
{
    public interface IHitUpgradeFactory
    {
        int GetUpgradeOrder();
        void IncreaseLevel();
        IHitResolver Create(IHitResolver inner);
    }
}

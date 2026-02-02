using Components.Managers;
using Components.Player.Upgrades;

namespace Components.UI
{
    public class UpgradeSelectorPresenter
    {
        private IGameFlowController gameFlow;
        private IUpgradeManager upgradeManager;
        private IUpgradeView upgradeView;

        public UpgradeSelectorPresenter(IGameFlowController gameFlow, IUpgradeManager upgradeManager, LevelModel levelModel, IUpgradeView upgradeView)
        {
            this.gameFlow = gameFlow;
            this.upgradeManager = upgradeManager;
            this.upgradeView = upgradeView;

            levelModel.LevelUp += SelectUpgrade;
        }

        private void SelectUpgrade(int amountUpgrades)
        {
            gameFlow.PauseGame();
            IHitUpgradeFactory[] randomUpgrades = upgradeManager.TryGetRandomUpgrades(amountUpgrades);
            upgradeView.EnableUpgradeView(randomUpgrades, UpgradeSelected);
        }

        private void UpgradeSelected(IHitUpgradeFactory upgradeFactory)
        {
            gameFlow.ResumeGame();
            upgradeView.DisableUpgradeView();
            upgradeManager.AddUpgrade(upgradeFactory);
        }
    }
}

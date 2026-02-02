using System;
using Components.Managers;
using Components.Player.Upgrades;
using UnityEngine;

namespace Components.UI
{
    public class GameBootstrapper : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private TimerView timerView;
        [SerializeField] private LevelView levelView;
        [SerializeField] private UpgradeView upgradeView;

        //Change this into ScriptableObject "GameSettings"
        [SerializeField] private float xpDiminishedPerLevel = 0.1f;
        [SerializeField] private float minimumDiminishedXp = 0.1f;
        [SerializeField] private int amountUpgradesAfterLevelUp = 3;
        [SerializeField] private float initialTime = 100;
        
        private ScoreModel scoreModel;
        private TimerModel timerModel;
        private LevelModel levelModel;
        private LiveGamePresenter liveGamePresenter;
        private UpgradeSelectorPresenter upgradeSelectorPresenter;

        [Inject] private IGameFlowController gameFlowController;
        [Inject] private IUpgradeManager upgradeManager;

        [Provide] private ScoreModel ProvideScoreModel()
        {
            return scoreModel ??= new ScoreModel();
        }
        [Provide] private TimerModel ProvideTimerModel()
        {
            return timerModel ??= new TimerModel(initialTime);
        }
        [Provide] private LevelModel ProvideLevelModel()
        {
            return levelModel ??= new LevelModel(xpDiminishedPerLevel, amountUpgradesAfterLevelUp, minimumDiminishedXp);
        }

        private void Awake()
        {
            scoreModel = ProvideScoreModel();
            timerModel = ProvideTimerModel();
            levelModel = ProvideLevelModel();
            
            liveGamePresenter = new LiveGamePresenter(scoreModel, scoreView, timerModel, timerView, levelModel, levelView);
            upgradeSelectorPresenter = new UpgradeSelectorPresenter(gameFlowController, upgradeManager, levelModel, upgradeView);
        }
    }
}

using System;
using Components.Managers;
using Components.ObjectPool;
using Components.Player.Upgrades;
using Components.UI.GameOver;
using Components.UI.Leaderboard;
using Components.UI.LiveGame;
using Components.UI.UpgradeSelection;
using UnityEngine;

namespace Components.UI
{
    public class GameBootstrapper : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private ScoreViewInstanceManager scoreViewInstanceManager;
        [SerializeField] private TimerView timerView;
        [SerializeField] private LevelView levelView;
        [SerializeField] private UpgradeView upgradeView;
        [SerializeField] private LeaderboardView leaderboardView;
        [SerializeField] private GameOverView gameOverView;

        [SerializeField] private GameSettings gameSettings;
        
        private ScoreModel scoreModel;
        private TimerModel timerModel;
        private LevelModel levelModel;
        private LeaderboardModel leaderboardModel;
        private LiveGamePresenter liveGamePresenter;
        private UpgradeSelectorPresenter upgradeSelectorPresenter;
        private GameOverPresenter gameOverPresenter;

        [Inject] private IGameFlowController gameFlowController;
        [Inject] private IUpgradeManager upgradeManager;
        [Inject] private IObjectPoolManager objectPoolManager;

        [Provide] private ScoreModel ProvideScoreModel()
        {
            return scoreModel ??= new ScoreModel();
        }
        [Provide] private TimerModel ProvideTimerModel()
        {
            return timerModel ??= new TimerModel(gameSettings.initialTime);
        }
        [Provide] private LevelModel ProvideLevelModel()
        {
            return levelModel ??= new LevelModel(gameSettings.xpDiminishedPerLevel, gameSettings.amountUpgradesAfterLevelUp, gameSettings.minimumDiminishedXp);
        }
        [Provide]
        private LeaderboardModel ProvideLeaderboardModel()
        {
            return leaderboardModel ??= new LeaderboardModel();
        }

        private void Awake()
        {
            scoreModel = ProvideScoreModel();
            timerModel = ProvideTimerModel();
            levelModel = ProvideLevelModel();
            leaderboardModel = ProvideLeaderboardModel();
            
            liveGamePresenter = new LiveGamePresenter(scoreModel, scoreView, scoreViewInstanceManager, timerModel, timerView, levelModel, levelView, objectPoolManager);
            upgradeSelectorPresenter = new UpgradeSelectorPresenter(gameFlowController, upgradeManager, levelModel, upgradeView);
            gameOverPresenter = new GameOverPresenter(gameOverView, leaderboardView, leaderboardModel, timerModel, new JsonLeaderboardStorage(), scoreModel, gameFlowController);
        }
    }
}

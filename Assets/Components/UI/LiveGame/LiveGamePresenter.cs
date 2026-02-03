using Components.ObjectPool;
using UnityEngine;

namespace Components.UI.LiveGame
{
    public class LiveGamePresenter
    {
        private IScoreView scoreView;
        private IScoreViewInstanceManager scoreViewInstanceManager;
        private ITimerView timerView;
        private ILevelView levelView;
        
        public LiveGamePresenter(ScoreModel scoreModel, IScoreView scoreView, IScoreViewInstanceManager scoreViewInstanceManager, TimerModel timerModel, ITimerView timerView, LevelModel levelModel, ILevelView levelView, IObjectPoolManager objectPoolManager)
        {
            this.scoreView = scoreView;
            this.scoreViewInstanceManager = scoreViewInstanceManager;
            this.timerView = timerView;
            this.levelView = levelView;

            this.scoreViewInstanceManager.SetPoolObjectManager(objectPoolManager);
            scoreModel.ScoreChanged += OnScoreChanged;
            scoreModel.ScoreAdded += OnAmountScoreAdded;
            timerModel.TimeChanged += OnTimerChanged;
            levelModel.LevelChanged += OnLevelChanged;
        }

        private void OnScoreChanged(int score)
        {
            scoreView.SetScore(score);
        }
        
        private void OnAmountScoreAdded(int score, Vector3 worldPos)
        {
            scoreViewInstanceManager.InstanceScore(score, worldPos);
        }

        private void OnTimerChanged(float time)
        {
            timerView.SetTimer(time);
        }

        private void OnLevelChanged(float level)
        {
            levelView.SetXp(level);
        }
    }
}

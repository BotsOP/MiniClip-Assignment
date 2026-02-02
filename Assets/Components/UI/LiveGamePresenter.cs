namespace Components.UI
{
    public class LiveGamePresenter
    {
        private IScoreView scoreView;
        private ITimerView timerView;
        private ILevelView levelView;
        
        public LiveGamePresenter(ScoreModel scoreModel, IScoreView scoreView, TimerModel timerModel, ITimerView timerView, LevelModel levelModel, ILevelView levelView)
        {
            this.scoreView = scoreView;
            this.timerView = timerView;
            this.levelView = levelView;

            scoreModel.ScoreChanged += OnScoreChanged;
            timerModel.TimeChanged += OnTimerChanged;
            levelModel.LevelChanged += OnLevelChanged;
        }

        private void OnScoreChanged(int score)
        {
            scoreView.SetScore(score);
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

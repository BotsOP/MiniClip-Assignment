using Components.Managers;
using Components.UI.GameOver;

namespace Components.UI.Leaderboard
{
    public class GameOverPresenter
    {
        private readonly IGameOverView gameOverView;
        private readonly ILeaderboardView leaderboardView;
        private readonly LeaderboardModel leaderboardModel;
        private readonly ScoreModel scoreModel;
        private readonly ILeaderboardStorage storage;
        private readonly IGameFlowController gameFlowController;

        public GameOverPresenter(IGameOverView gameOverView, ILeaderboardView leaderboardView, LeaderboardModel leaderboardModel, TimerModel timerModel, ILeaderboardStorage storage, ScoreModel scoreModel, IGameFlowController gameFlowController)
        {
            leaderboardModel.SetStorage(storage);
            
            this.gameOverView = gameOverView;
            this.leaderboardView = leaderboardView;
            this.leaderboardModel = leaderboardModel;
            this.scoreModel = scoreModel;
            this.storage = storage;
            this.gameFlowController = gameFlowController;

            timerModel.TimeFinished += GameOver;
            gameOverView.AddedLeaderboardEntry += AddEntry;
            gameOverView.RestartGame += RestartGame;
        }

        private void GameOver()
        {
            gameFlowController.PauseGame();
            gameOverView.EnableView(scoreModel.Score);
            leaderboardView.ShowEntries(storage.Load());
        }

        private void AddEntry(string playerName)
        {
            leaderboardModel.AddEntry(playerName, scoreModel.Score);
            leaderboardView.ShowEntries(storage.Load());
        }

        private void RestartGame()
        {
            gameFlowController.RestartScene();
            gameFlowController.ResumeGame();
        }
    }

}

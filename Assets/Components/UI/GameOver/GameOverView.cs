using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI.GameOver
{
    public interface IGameOverView
    {
        event Action<string> AddedLeaderboardEntry;
        event Action RestartGame;
        void EnableView(int score);
    }

    public class GameOverView : MonoBehaviour, IGameOverView
    {
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private Button addScoreToLeaderboard;
        [SerializeField] private Button restartGame;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private TMP_Text scoreText;

        public event Action<string> AddedLeaderboardEntry;
        public event Action RestartGame;

        private void Awake()
        {
            addScoreToLeaderboard.onClick.AddListener((() => AddedLeaderboardEntry?.Invoke(playerNameInputField.text)));
            restartGame.onClick.AddListener((() => RestartGame?.Invoke()));
        }

        public void EnableView(int score)
        {
            gameOverScreen.SetActive(true);
            scoreText.text = score.ToString();
        }
        
    }
}

using TMPro;
using UnityEngine;

namespace Components.UI.Leaderboard
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text scoreText;
        public void Set(string playerName, int score)
        {
            playerNameText.text = playerName;
            scoreText.text = score.ToString();
        }
    }
}

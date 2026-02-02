using TMPro;
using UnityEngine;

namespace Components.UI
{
    public interface IScoreView
    {
        void SetScore(int score);
    }

    public class ScoreView : MonoBehaviour, IScoreView
    {
        [SerializeField] private TMP_Text scoreText;

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}

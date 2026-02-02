using System;

namespace Components.UI
{
    public class ScoreModel
    {
        public event Action<int> ScoreChanged;
        public int Score { get; private set; }

        public void AddScore(int amount)
        {
            Score += amount;
            ScoreChanged?.Invoke(Score);
        }
    }
}

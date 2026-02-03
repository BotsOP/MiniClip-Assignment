using System;
using UnityEngine;

namespace Components.UI
{
    public class ScoreModel
    {
        public event Action<int> ScoreChanged;
        public event Action<int, Vector3> ScoreAdded;
        public int Score { get; private set; }

        public void AddScore(int amount, Vector3 worldPos)
        {
            Score += amount;
            ScoreChanged?.Invoke(Score);
            ScoreAdded?.Invoke(amount, worldPos);
        }
    }
}

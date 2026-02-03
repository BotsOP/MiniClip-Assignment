using System.Collections.Generic;
using UnityEngine;

namespace Components.UI.Leaderboard
{
    public class LeaderboardModel
    {
        private ILeaderboardStorage storage;
        private List<LeaderboardEntry> entries;

        public IReadOnlyList<LeaderboardEntry> Entries => entries;

        public void SetStorage(ILeaderboardStorage storage)
        {
            this.storage = storage;
            entries = storage.Load();
        }

        public void AddEntry(string name, int score)
        {
            if (entries is null)
            {
                Debug.LogError($"Forgot to call SetStorage. Storage is null");
                return;
            }
            
            entries.Add(new LeaderboardEntry(name, score));
            entries.Sort((a, b) => b.score.CompareTo(a.score));
            storage.Save(entries);
        }
    }
}

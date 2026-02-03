using System.Collections.Generic;
using UnityEngine;

namespace Components.UI.Leaderboard
{
    public interface ILeaderboardView
    {
        void ShowEntries(List<LeaderboardEntry> entries);
    }
    
    public class LeaderboardView : MonoBehaviour, ILeaderboardView
    {
        [SerializeField] private Transform contentRoot;
        [SerializeField] private LeaderboardEntryView entryPrefab;

        public void ShowEntries(List<LeaderboardEntry> entries)
        {
            for (int i = 0; i < contentRoot.childCount; i++)
            {
                Destroy(contentRoot.GetChild(i).gameObject);
            }

            foreach (LeaderboardEntry entry in entries)
            {
                LeaderboardEntryView view = Instantiate(entryPrefab, contentRoot);
                view.Set(entry.playerName, entry.score);
            }
        }
    }

}

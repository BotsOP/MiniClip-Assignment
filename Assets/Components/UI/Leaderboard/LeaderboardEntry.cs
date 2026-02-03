using System;

namespace Components.UI.Leaderboard
{
    [Serializable]
    public struct LeaderboardEntry
    {
        public string playerName;
        public int score;
        public long timestamp;

        public LeaderboardEntry(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}

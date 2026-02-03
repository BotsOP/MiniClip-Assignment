using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Components.UI.Leaderboard
{
    public interface ILeaderboardStorage
    {
        void Save(List<LeaderboardEntry> entries);
        List<LeaderboardEntry> Load();
    }
    
    public class JsonLeaderboardStorage : ILeaderboardStorage
    {
        private readonly string filePath;

        public JsonLeaderboardStorage(string fileName = "leaderboard.json")
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void Save(List<LeaderboardEntry> entries)
        {
            string json = JsonUtility.ToJson(new Wrapper { entries = entries }, true);
            File.WriteAllText(filePath, json);
        }

        public List<LeaderboardEntry> Load()
        {
            if (!File.Exists(filePath))
                return new List<LeaderboardEntry>();

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Wrapper>(json).entries;
        }

        [Serializable]
        private class Wrapper
        {
            public List<LeaderboardEntry> entries;
        }
    }
}

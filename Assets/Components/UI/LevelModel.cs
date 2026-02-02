using System;
using UnityEngine;

namespace Components.UI
{
    public class LevelModel
    {
        public event Action<float> LevelChanged;
        public event Action<int> LevelUp;
        public float Level { get; private set; }
        private readonly float xpDiminishedPerLevel;
        private readonly float minimumDiminishedXp;
        private readonly int amountUpgradesAfterLevelUp;
        public LevelModel(float xpDiminishedPerLevel, int amountUpgradesAfterLevelUp, float minimumDiminishedXp)
        {
            this.xpDiminishedPerLevel = xpDiminishedPerLevel;
            this.minimumDiminishedXp = minimumDiminishedXp;
            this.amountUpgradesAfterLevelUp = amountUpgradesAfterLevelUp;
        }

        public void AddXp(float amount)
        {
            int levelBefore = Mathf.FloorToInt(Level);
            float diminishedXp = Mathf.Clamp(Mathf.Pow(Mathf.FloorToInt(amount), 1 - xpDiminishedPerLevel * levelBefore), minimumDiminishedXp, 1);
            Level += amount * diminishedXp;
            int levelAfter = Mathf.FloorToInt(Level);
            LevelChanged?.Invoke(Level);

            if (levelAfter > levelBefore)
            {
                LevelUp?.Invoke(amountUpgradesAfterLevelUp);
            }
        }
    }
}

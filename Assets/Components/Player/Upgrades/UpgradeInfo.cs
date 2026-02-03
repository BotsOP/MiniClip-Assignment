using UnityEngine;

namespace Components.Player.Upgrades
{
    public struct UpgradeInfo
    {
        public int level;
        public string nameUpgrade;
        public Texture2D textureUpgrade;
        public UpgradeStatInfo[] upgradeStatInfos;

        public UpgradeInfo(int level, string nameUpgrade, Texture2D textureUpgrade, UpgradeStatInfo[] upgradeStatInfos)
        {
            this.level = level;
            this.nameUpgrade = nameUpgrade;
            this.textureUpgrade = textureUpgrade;
            this.upgradeStatInfos = upgradeStatInfos;
        }
    }
    
    public struct UpgradeStatInfo
    {
        public string nameStatModified;
        public float valueBefore;
        public float valueAfter;
    }
}

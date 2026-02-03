using System;
using Components.Player.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI.UpgradeSelection
{
    public class UpgradeSelectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text upgradeNameText;
        [SerializeField] private TMP_Text upgradeLevelText;
        [SerializeField] private RawImage upgradeImage;
        [SerializeField] private UpgradeStatView upgradeStatPrefab;
        [SerializeField] private Transform upgradeStatLayout;
        [SerializeField] private Button selectButton;

        public void SetUpgradeInfo(IHitUpgradeFactory upgradeFactory, Action<IHitUpgradeFactory> upgradeSelected)
        {
            selectButton.onClick.AddListener((() => upgradeSelected?.Invoke(upgradeFactory)));
            
            UpgradeInfo upgradeInfo = upgradeFactory.GetUpgradeInfo();
            upgradeNameText.text = upgradeInfo.nameUpgrade;
            upgradeLevelText.text = upgradeInfo.level.ToString();
            upgradeImage.texture = upgradeInfo.textureUpgrade;
            
            foreach (UpgradeStatInfo upgradeInfoUpgradeStatInfo in upgradeInfo.upgradeStatInfos)
            {
                UpgradeStatView upgradeStat = Instantiate(upgradeStatPrefab, upgradeStatLayout);
                upgradeStat.SetUpgradeStatInfo(upgradeInfoUpgradeStatInfo);
            }
        }
    }
}

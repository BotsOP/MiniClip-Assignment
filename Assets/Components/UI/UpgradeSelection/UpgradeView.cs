using System;
using Components.Player.Upgrades;
using UnityEngine;

namespace Components.UI.UpgradeSelection
{
    public interface IUpgradeView
    {
        void EnableUpgradeView(IHitUpgradeFactory[] upgradeInfos, Action<IHitUpgradeFactory> upgradeSelected);
        void DisableUpgradeView();
    }

    public class UpgradeView : MonoBehaviour, IUpgradeView
    {
        [SerializeField] private GameObject upgradeScreen;
        [SerializeField] private UpgradeSelectionView upgradeSelectionViewPrefab;
        [SerializeField] private Transform upgradeSelectionLayout;

        public void EnableUpgradeView(IHitUpgradeFactory[] upgradeInfos, Action<IHitUpgradeFactory> upgradeSelected)
        {
            upgradeScreen.SetActive(true);
            foreach (IHitUpgradeFactory upgradeInfo in upgradeInfos)
            {
                UpgradeSelectionView upgradeSelection = Instantiate(upgradeSelectionViewPrefab, upgradeSelectionLayout);
                upgradeSelection.SetUpgradeInfo(upgradeInfo, upgradeSelected);
            }
        }

        public void DisableUpgradeView()
        {
            upgradeScreen.SetActive(false);

            for (int i = 0; i < upgradeSelectionLayout.childCount; i++)
            {
                Destroy(upgradeSelectionLayout.GetChild(i).gameObject);
            }
        }
    }
}

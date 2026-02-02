using Components.Player.Upgrades;
using TMPro;
using UnityEngine;

namespace Components.UI
{
    public class UpgradeStatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameStatText;
        [SerializeField] private TMP_Text valueBeforeText;
        [SerializeField] private TMP_Text valueAfterText;

        public void SetUpgradeStatInfo(UpgradeStatInfo upgradeStatInfo)
        {
            nameStatText.text = upgradeStatInfo.nameStatModified;
            valueBeforeText.text = upgradeStatInfo.valueBefore.ToString();
            valueAfterText.text = upgradeStatInfo.valueAfter.ToString();
        }
    }
}

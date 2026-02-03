using UnityEngine;

namespace Components.Managers
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] public float xpDiminishedPerLevel = 0.1f;
        [SerializeField] public float minimumDiminishedXp = 0.1f;
        [SerializeField] public int amountUpgradesAfterLevelUp = 3;
        [SerializeField] public float initialTime = 100;
    }
}

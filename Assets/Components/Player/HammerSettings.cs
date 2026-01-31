using UnityEngine;

namespace Components.Player
{
    [CreateAssetMenu(fileName = "HammerSettings", menuName = "Player/HammerSettings")]
    public class HammerSettings : ScriptableObject
    {
        [SerializeField] public Transform hammerTransform;
        [SerializeField, Range(1, 20)] public float hammerPowerDistanceScaling = 1;
        [SerializeField, Min(0.01f)] public float maxHammerPower = 0.01f;
        [SerializeField, Range(1, 270)] public float hammerRotationScaling = 1;
        [SerializeField, Range(1, 100)] public float damageScaling = 1;
        [SerializeField] public LayerMask groundLayerMask;

        public HammerData GetHammerData()
        {
            return new HammerData() {
                damageScaling = damageScaling,
                hammerPowerDistanceScaling = hammerPowerDistanceScaling,
                maxHammerPower = maxHammerPower,
            };
        }
    }

    public struct HammerData
    {
        public float hammerPowerDistanceScaling;
        public float maxHammerPower;
        public float damageScaling; 
    }
}

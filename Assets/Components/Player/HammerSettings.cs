using UnityEngine;

namespace Components.Player
{
    [CreateAssetMenu(fileName = "HammerSettings", menuName = "Player/HammerSettings")]
    public class HammerSettings : ScriptableObject
    {
        [SerializeField] public Transform hammerTransform;
        [SerializeField, Range(1, 100)] public float damage = 1;
        [SerializeField] public LayerMask groundLayerMask;

        public HammerData GetHammerData()
        {
            return new HammerData() {
                damage = damage,
            };
        }
    }

    public struct HammerData
    {
        public Vector3 worldPos;
        public Transform pivot;
        public float damage;
    }
}

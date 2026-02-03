using Components.ObjectPool;
using UnityEngine;

namespace Components.Player
{
    [CreateAssetMenu(fileName = "HammerSettings", menuName = "Player/HammerSettings")]
    public class HammerSettings : ScriptableObject
    {
        [SerializeField] public Transform hammerPrefab;
        [SerializeField] public PoolObject extraHammerPrefab;
        [SerializeField, Range(1, 100)] public float damage = 1;
        [SerializeField] public LayerMask groundLayerMask;

        public HammerData GetHammerData()
        {
            return new HammerData() {
                extraHammerInstance = extraHammerPrefab,
                damage = damage,
            };
        }
    }

}

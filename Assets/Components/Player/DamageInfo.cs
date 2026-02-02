using UnityEngine;

namespace Components.Player
{
    public struct DamageInfo
    {
        public Vector3 worldPos;
        public readonly float damage;
        
        public DamageInfo(Vector3 worldPos, float damage)
        {
            this.worldPos = worldPos;
            this.damage = damage;
        }
    }
}

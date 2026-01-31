using UnityEngine;

namespace Components.Player
{
    public struct DamageInfo
    {
        public Vector3 worldPos;
        public float damage;
        public Vector2Int gridOffset;
        
        public DamageInfo(Vector3 worldPos, float damage, Vector2Int gridOffset)
        {
            this.worldPos = worldPos;
            this.damage = damage;
            this.gridOffset = gridOffset;
        }
        
        public DamageInfo(Vector3 worldPos, float damage)
        {
            this.worldPos = worldPos;
            this.damage = damage;
            gridOffset = new Vector2Int(0, 0);
        }
    }
}

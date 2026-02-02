using UnityEngine;

namespace Components.Player
{
    public enum DamageSource
    {
        Player,
        Mole,
    }
    public struct DamageInfo
    {
        public Vector3 worldPos;
        public Vector2Int offset;
        public readonly float damage;
        public DamageSource damageSource;

        public DamageInfo(Vector3 worldPos, Vector2Int offset, float damage, DamageSource damageSource)
        {
            this.worldPos = worldPos;
            this.offset = offset;
            this.damage = damage;
            this.damageSource = damageSource;
        }
    }
}

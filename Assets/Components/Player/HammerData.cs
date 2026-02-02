using System;
using Components.ObjectPool;
using UnityEngine;

namespace Components.Player
{
    public struct HammerData
    {
        public delegate bool SpawnHammer(
            PoolObject prefab,
            out PoolObject instance
        );
        public delegate bool ReleaseHammer(
            PoolObject prefab
        );

        public ReleaseHammer release;
        public SpawnHammer spawn;
        public PoolObject extraHammerInstance;
        public Transform pivot;
        public Vector3 worldPos;
        public float damage;
    }
}

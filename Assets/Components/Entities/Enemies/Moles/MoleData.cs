using System;
using Components.ObjectPool;
using Components.UI;
using UnityEngine;

namespace Components.Entities.Enemies.Moles
{
    [CreateAssetMenu(fileName = "BasicMole", menuName = "Moles/BasicMole")]
    public class MoleData : ScriptableObject, IMoleFactory
    {
        [Header("Spawning")]
        public int difficulty;
        public float spawnChance;
        
        [Header("Mole Settings")]
        public MoleViewer molePrefab;
        public float leaveTime = 1;
        public float maxHealth = 10;
        public int score = 10;
        public float xp = 0.1f;
        
        [HideInInspector] public float scaledSpawnChance;
        public int GetDifficulty()
        {
            return difficulty;
        }
        public float GetSpawnChance()
        {
            return spawnChance;
        }
        public void SetScaledDifficulty(float scaledSpawnChance)
        {
            this.scaledSpawnChance = scaledSpawnChance;
        }
        public MoleViewer GetMoleViewer()
        {
            return molePrefab;
        }
        public virtual Mole Create(Action<Entity> enemyDiedCallback, IObjectPoolManager objectPool, ScoreModel scoreModel, LevelModel levelModel)
        {
            MoleViewer instance = SpawnMolePrefab(objectPool);

            return new Mole(enemyDiedCallback, maxHealth, score, xp, leaveTime, instance, scoreModel, levelModel);
        }
        
        protected MoleViewer SpawnMolePrefab(IObjectPoolManager objectPool)
        {
            if (!objectPool.Spawn(molePrefab, out PoolObject instance))
            {
                Debug.LogError("Failed spawning mole");
            }
            return (MoleViewer)instance;
        }
    }
}

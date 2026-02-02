using System;
using System.Collections.Generic;
using System.Linq;
using Components.ObjectPool;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.Entities.Enemies.Moles
{
    public enum TypeOfMole
    {
        Mole,
    }
    public class MoleSpawner : MonoBehaviour, IEnemySpawner, IDependencyProvider
    {
        [SerializeField] private MoleData[] moleDatas;
        
        private readonly List<Mole> moles = new List<Mole>();
        private Dictionary<int, List<MoleData>> indexedMolesChances = new Dictionary<int, List<MoleData>>();
        private int maxDifficulty;
        [Inject] private ObjectPoolManager objectPoolManager;

        [Provide]
        public IEnemySpawner ProvideEnemySpawner()
        {
            return this;
        }

        private void OnEnable()
        {
            foreach (MoleData moleData in moleDatas)
            {
                maxDifficulty = moleData.difficulty > maxDifficulty ? moleData.difficulty : maxDifficulty;
                
                if (!indexedMolesChances.ContainsKey(moleData.difficulty))
                    indexedMolesChances.Add(moleData.difficulty, new List<MoleData> { moleData });
                else
                    indexedMolesChances[moleData.difficulty].Add(moleData);

                objectPoolManager.CreatePool(moleData.moleInstance);
            }

            foreach (List<MoleData> moleDataList in indexedMolesChances.Values)
            {
                ScaleSpawnChances(moleDataList);
            }
        }
        
        private static void ScaleSpawnChances(List<MoleData> moleAndChances)
        {
            float maxSpawnChance = moleAndChances.Sum(moleAndChance => moleAndChance.spawnChance);
            
            moleAndChances.Sort((a, b) => b.spawnChance.CompareTo(a.spawnChance));
            
            float cachedSpawnChance = 0;
            foreach (MoleData moleAndChance in moleAndChances)
            {
                cachedSpawnChance += moleAndChance.spawnChance / maxSpawnChance;
                moleAndChance.scaledSpawnChance = cachedSpawnChance;
            }
        }

        public bool SpawnEnemy(Vector3 spawnPosition, int difficulty, Action<Entity> enemyDiedCallback, out Enemy enemy)
        {
            difficulty = Mathf.Clamp(difficulty, 0, maxDifficulty);
            Mole mole = GetRandomMole(difficulty, spawnPosition, enemyDiedCallback);
            enemy = mole;
            
            if (mole == null)
                return false;
            
            mole.AddDiedCallback(RemoveMole);
            moles.Add(mole);
            return true;
        }

        public void Update()
        {
            for (int i = 0; i < moles.Count; i++)
            {
                Mole mole = moles[i];
                
                mole.Update();
            }
        }

        private Mole GetRandomMole(int difficulty, Vector3 spawnPosition, Action<Entity> enemyDiedCallback)
        {
            float random = Random.Range(0.0f, 1.0f);
            Mole mole = null;
            for (int i = 0; i < indexedMolesChances[difficulty].Count; i++)
            {
                MoleData moleData = indexedMolesChances[difficulty][i];
                if ((random > moleData.scaledSpawnChance))
                    continue;
                
                mole = GetMoleFromType(moleData, spawnPosition, enemyDiedCallback);
                break;
            }
            return mole;
        }

        private Mole GetMoleFromType(MoleData moleData, Vector3 spawnPosition, Action<Entity> enemyDiedCallback)
        {
            switch (moleData.typeOfMole)
            {
                case TypeOfMole.Mole :
                    if (!objectPoolManager.Spawn(moleData.moleInstance, out PoolObject instance))
                    {
                        Debug.LogError("Failed spawning mole");
                    }
                    instance.transform.position = spawnPosition;
                    
                    return new Mole(enemyDiedCallback, moleData, (MoleViewer)instance);
                default:
                    return null;
            }
        }

        private void RemoveMole(Entity entity)
        {
            Mole mole = (Mole)entity;
            objectPoolManager.Release(mole.MoleViewer.GetPoolObject());
            moles.Remove(mole);
        }
    }

}
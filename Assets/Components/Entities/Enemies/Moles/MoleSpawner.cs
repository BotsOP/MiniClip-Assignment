using System;
using System.Collections.Generic;
using System.Linq;
using Components.ObjectPool;
using Components.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components.Entities.Enemies.Moles
{
    public class MoleSpawner : MonoBehaviour, IEnemySpawner, IDependencyProvider
    {
        [SerializeField] private List<ScriptableObject> moleDatas;
        
        private readonly List<Mole> moles = new List<Mole>();
        private readonly Dictionary<int, List<IMoleFactory>> indexedMolesChances = new Dictionary<int, List<IMoleFactory>>();
        
        private List<IMoleFactory> moleFactories;
        private int maxDifficulty;
        private int lastUsedDifficulty;
        
        [Inject] private IObjectPoolManager objectPoolManager;
        [Inject] private ScoreModel scoreModel;
        [Inject] private LevelModel levelModel;
        
        [Provide] public IEnemySpawner ProvideEnemySpawner()
        {
            return this;
        }

        private void OnValidate()
        {
            if (moleDatas == null)
                return;

            for (int i = moleDatas.Count - 1; i >= 0; i--)
            {
                if (!EnsureScriptableObjectIsUpgrade(moleDatas[i]))
                {
                    moleDatas.RemoveAt(i);
                    i--;
                }
            }
        }
        
        private bool EnsureScriptableObjectIsUpgrade(ScriptableObject so)
        {
            if (so == null)
                return false;
                
            if (so is not IMoleFactory)
            {
                Debug.LogError($"{so.name} does not implement {nameof(IMoleFactory)} and was removed", this);
                return false;
            }
            
            return true;
        }

        private void OnEnable()
        {
            moleFactories = new List<IMoleFactory>();
            for (int i = moleDatas.Count - 1; i >= 0; i--)
            {
                if (!EnsureScriptableObjectIsUpgrade(moleDatas[i]))
                {
                    moleDatas.RemoveAt(i);
                    i--;
                }
                moleFactories.Add((IMoleFactory)moleDatas[i]);
            }

            int minDifficulty = int.MaxValue;
            foreach (IMoleFactory moleFactory in moleFactories)
            {
                maxDifficulty = moleFactory.GetDifficulty() > maxDifficulty ? moleFactory.GetDifficulty() : maxDifficulty;
                minDifficulty = moleFactory.GetDifficulty() < minDifficulty ? moleFactory.GetDifficulty() : minDifficulty;
                
                if (!indexedMolesChances.ContainsKey(moleFactory.GetDifficulty()))
                    indexedMolesChances.Add(moleFactory.GetDifficulty(), new List<IMoleFactory> { moleFactory });
                else
                    indexedMolesChances[moleFactory.GetDifficulty()].Add(moleFactory);

                objectPoolManager.CreatePool(moleFactory.GetMoleViewer());
            }
            lastUsedDifficulty = minDifficulty;

            foreach (List<IMoleFactory> moleDataList in indexedMolesChances.Values)
            {
                ScaleSpawnChances(moleDataList);
            }
        }
        
        private static void ScaleSpawnChances(List<IMoleFactory> moleAndChances)
        {
            float maxSpawnChance = moleAndChances.Sum(moleAndChance => moleAndChance.GetSpawnChance());
            
            moleAndChances.Sort((a, b) => b.GetSpawnChance().CompareTo(a.GetSpawnChance()));
            
            float cachedSpawnChance = 0;
            foreach (IMoleFactory moleAndChance in moleAndChances)
            {
                cachedSpawnChance += moleAndChance.GetSpawnChance() / maxSpawnChance;
                moleAndChance.SetScaledDifficulty(cachedSpawnChance);
            }
        }

        public bool SpawnEnemy(Vector3 spawnPosition, int difficulty, Action<Entity> enemyDiedCallback, out Enemy enemy)
        {
            difficulty = Mathf.Clamp(difficulty, 0, maxDifficulty);
            if (!indexedMolesChances.ContainsKey(difficulty))
            {
                Debug.LogError($"No mole registered with difficulty {difficulty}");
                difficulty = lastUsedDifficulty;
            }
            Mole mole = GetRandomMole(difficulty, spawnPosition, enemyDiedCallback);
            enemy = mole;
            
            if (mole == null)
                return false;
            
            mole.AddDiedCallback(RemoveMole);
            moles.Add(mole);
            lastUsedDifficulty = difficulty;
            return true;
        }

        public void Update()
        {
            foreach (Mole mole in moles)
            {
                mole.Update();
            }
        }

        private Mole GetRandomMole(int difficulty, Vector3 spawnPosition, Action<Entity> enemyDiedCallback)
        {
            float random = Random.Range(0.0f, 1.0f);
            Mole mole = null;
            for (int i = 0; i < indexedMolesChances[difficulty].Count; i++)
            {
                IMoleFactory moleData = indexedMolesChances[difficulty][i];
                if ((random > moleData.GetSpawnChance()))
                    continue;
                
                mole = GetMoleFromType(moleData, spawnPosition, enemyDiedCallback);
                break;
            }
            return mole;
        }

        private Mole GetMoleFromType(IMoleFactory moleFactory, Vector3 spawnPosition, Action<Entity> enemyDiedCallback)
        {
            Mole mole = moleFactory.Create(enemyDiedCallback, objectPoolManager, scoreModel, levelModel);
            mole.MoleViewer.transform.position = spawnPosition;
            return mole;
        }

        private void RemoveMole(Entity entity)
        {
            Mole mole = (Mole)entity;
            objectPoolManager.Release(mole.MoleViewer.GetPoolObject());
            moles.Remove(mole);
        }
    }
}
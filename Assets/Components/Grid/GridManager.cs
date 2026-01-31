using System;
using System.Collections.Generic;
using Components.Entities;
using Components.Entities.Enemies;
using Components.Managers;
using Components.Player;
using Managers;
using UnityEngine;
using EventType = Managers.EventType;
using Random = UnityEngine.Random;

namespace Components.Grid
{
    public interface IGridManager
    {
        void SpawnRandomEnemy(int difficulty);
    }

    public class GridManager : MonoBehaviour, IGridManager, IDependencyProvider
    {
        [SerializeField, Min(1)] private int gridWidth = 1;
        [SerializeField, Min(1)] private int gridHeight = 1;
        [SerializeField, Min(1)] private float cellSize = 1;
        
        [Inject] private IEnemySpawner enemySpawner;

        private Entity[,] grid;

        [Provide]
        public IGridManager ProvideGridManager()
        {
            return this;
        }
        
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < gridHeight; j++)
            {
                Vector3 position = IndexToWorldPos(i, j);
                Gizmos.DrawWireCube(position, new Vector3(cellSize, 0, cellSize));
            }
        }

        private void OnEnable()
        {
            grid = new Entity[gridWidth, gridHeight];
            EventSystem<int>.Subscribe(EventType.SpawnRandomEnemy, SpawnRandomEnemy);
            EventSystem<DamageInfo>.Subscribe(EventType.DoDamage, DoDamage);
        }
        private void OnDisable()
        {
            EventSystem<int>.UnSubscribe(EventType.SpawnRandomEnemy, SpawnRandomEnemy);
            EventSystem<DamageInfo>.UnSubscribe(EventType.DoDamage, DoDamage);
        }

        private void DoDamage(DamageInfo damageInfo)
        {
            Vector2Int index = WorldPosToIndex(damageInfo.worldPos);
            index += damageInfo.gridOffset;
            
            if (index.x < 0 || index.y < 0 || index.x >= gridWidth || index.y >= gridHeight)
            {
                Debug.LogError($"Index: {index} is out of bounds");
                return;
            }
            
            grid[index.x, index.y]?.DoDamage(damageInfo.damage);
        }

        public void SpawnRandomEnemy(int difficulty)
        {
            if (!GetRandomFreeIndex(out Vector2Int freeIndex))
            {
                Debug.Log("Couldn't find free spot");
                return;
            }
            
            Vector3 spawnPosition = IndexToWorldPos(freeIndex);
            if (!enemySpawner.SpawnEnemy(spawnPosition, difficulty, FreeUpGridSpot, out Enemy enemy))
            {
                Debug.LogError("Failed to spawn enemy");
                return;
            }
            
            grid[freeIndex.x, freeIndex.y] = enemy;
        }

        private bool GetRandomFreeIndex(out Vector2Int freeIndex)
        {
            List<Vector2Int> availablePositions = new List<Vector2Int>();
            for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < gridHeight; j++)
            {
                if (grid[i, j] == null)
                {
                    availablePositions.Add(new Vector2Int(i, j));
                }
            }
            if (availablePositions.Count == 0)
            {
                freeIndex = Vector2Int.zero;
                return false;
            }
            int randomIndex = Random.Range(0, availablePositions.Count);
            freeIndex = availablePositions[randomIndex];
            return true;
        }

        private void FreeUpGridSpot(Entity entity)
        {
            for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < gridHeight; j++)
            {
                if (grid[i, j] == entity)
                {
                    grid[i, j] = null;
                }
            }
        }

        private Vector2Int WorldPosToIndex(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - GetBottomLeftCorner() + new Vector3(cellSize / 2, 0, cellSize / 2);
    
            int x = Mathf.FloorToInt(localPos.x / cellSize);
            int z = Mathf.FloorToInt(localPos.z / cellSize);
    
            Debug.Log($"{worldPos} {x} {z}");
            return new Vector2Int(x, z);
        }

        private Vector3 IndexToWorldPos(int x, int y)
        {
            Vector3 worldPos = new Vector3(x * cellSize, 0, y * cellSize);
            worldPos += GetBottomLeftCorner();
            return worldPos;
        }
        
        private Vector3 IndexToWorldPos(Vector2Int index)
        {
            Vector3 worldPos = new Vector3(index.x * cellSize, 0, index.y * cellSize);
            worldPos += GetBottomLeftCorner();
            return worldPos;
        }

        private Vector3 GetBottomLeftCorner()
        {
            bool evenWidth = gridWidth % 2 == 0;
            bool evenHeight = gridHeight % 2 == 0;

            float leftX = (int)(gridWidth / 2) * cellSize;
            if (evenWidth)
                leftX -= cellSize / 2;
            
            float bottomZ = (int)(gridHeight / 2) * cellSize;
            if (evenHeight)
                bottomZ -= cellSize / 2;

            return new Vector3(-leftX, 0, -bottomZ);
        }
    }
}

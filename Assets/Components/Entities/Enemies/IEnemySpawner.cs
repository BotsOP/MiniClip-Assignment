using System;
using UnityEngine;

namespace Components.Entities.Enemies
{
    public interface IEnemySpawner
    {
        public bool SpawnEnemy(Vector3 spawnPosition, int difficulty, Action<Entity> enemyDiedCallback, out Enemy enemy);
    }
}

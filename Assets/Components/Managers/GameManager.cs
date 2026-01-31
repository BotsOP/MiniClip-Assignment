using System;
using System.Threading;
using Components.Grid;
using Managers;
using UnityEngine;
using EventType = Managers.EventType;

namespace Components.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float spawnEnemyDelay = 1;
        
        private int difficulty;
        private float timer;
        [Inject] private IGridManager gridManager;
        private void OnEnable()
        {
            EventSystem<int>.Subscribe(EventType.AddScore, AddScore);
        }
        private void OnDisable()
        {
            EventSystem<int>.UnSubscribe(EventType.AddScore, AddScore);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= spawnEnemyDelay)
            {
                timer -= spawnEnemyDelay;
                gridManager.SpawnRandomEnemy(difficulty);
            }
        }

        private void AddScore(int score)
        {
            
        }
    }
}

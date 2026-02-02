using System;
using System.Threading;
using Components.Grid;
using Components.UI;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Managers
{
    public interface IGameFlowController
    {
        void PauseGame();
        void ResumeGame();
    }

    public class GameManager : MonoBehaviour, IDependencyProvider, IGameFlowController
    {
        [SerializeField] private float spawnEnemyDelay = 1;
        
        private int difficulty;
        private float spawnTimer;
        private float gameTimer;
        private bool paused;
        [Inject] private IGridManager gridManager;
        [Inject] private TimerModel timerModel;

        [Provide] private IGameFlowController ProvideGameFlowController()
        {
            return this;
        }

        private void Update()
        {
            UpdateTimer();
            SpawnEnemyTimer();
        }
        
        private void SpawnEnemyTimer()
        {
            if (paused)
                return;
            
            spawnTimer += Time.deltaTime;

            if (!(spawnTimer >= spawnEnemyDelay) )
                return;
            
            spawnTimer -= spawnEnemyDelay;
            gridManager.SpawnRandomEnemy(difficulty);
        }

        private void UpdateTimer()
        {
            if (paused)
                return;
            
            timerModel.IncrementTime(-Time.deltaTime);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            paused = true;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
}

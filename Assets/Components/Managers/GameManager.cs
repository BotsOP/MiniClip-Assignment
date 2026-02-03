using System;
using System.Threading;
using Components.Grid;
using Components.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Components.Managers.EventType;

namespace Components.Managers
{
    public interface IGameFlowController
    {
        void PauseGame();
        void ResumeGame();
        void RestartScene();
    }

    public class GameManager : MonoBehaviour, IDependencyProvider, IGameFlowController
    {
        [SerializeField] private float spawnEnemyDelay = 1;
        
        private int difficulty;
        private float spawnTimer;
        private float gameTimer;
        private bool paused;
        [Inject] private ISpawnManager spawnManager;
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
            spawnManager.SpawnRandomEnemy(difficulty);
        }

        private void UpdateTimer()
        {
            if (paused)
                return;
            
            timerModel.IncrementTime(-Time.deltaTime);
        }

        public void PauseGame()
        {
            EventSystem<bool>.RaiseEvent(EventType.PausedGame, true);
            Time.timeScale = 0;
            paused = true;
        }

        public void ResumeGame()
        {
            EventSystem<bool>.RaiseEvent(EventType.PausedGame, false);
            Time.timeScale = 1;
            paused = false;
        }
        
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

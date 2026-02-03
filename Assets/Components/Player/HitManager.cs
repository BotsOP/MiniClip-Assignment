using Components.Grid;
using Components.Managers;
using Components.ObjectPool;
using Components.Player.Upgrades;
using UnityEngine;
using EventType = Components.Managers.EventType;

namespace Components.Player
{
    public class HitManager : MonoBehaviour
    {
        [SerializeField] private HammerSettings hammerSettings;
        
        [Inject] private IInputManager inputManager;
        [Inject] private IUpgradeManager upgradeManager;
        [Inject] private IObjectPoolManager objectPool;
        [Inject] private IDamageManager damageManager;

        private Transform hammerTransform;
        private Transform hammerPivot;
        private HammerData hammerData;
        private Camera mainCamera;
        private Vector2 cachedScaledStartScreenPos;
        private IHitResolver hit;
        private bool pausedGame;
        
        private Quaternion BaseHammerRotation => cachedScaledStartScreenPos.x < 0.5 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        private void OnEnable()
        {
            inputManager.StartHit += StartHit;
            upgradeManager.UpdateHit += SetHit;

            objectPool.CreatePool(hammerSettings.extraHammerPrefab, new DefaultPoolLifecycleStrategy());
        
            mainCamera = Camera.main;
            hammerTransform = Instantiate(hammerSettings.hammerPrefab);
            hammerPivot = hammerTransform.GetChild(0);
            
            hammerData = hammerSettings.GetHammerData();
            hammerData.baseHammerTransform = hammerTransform;
            hammerData.spawn = objectPool.Spawn;
            hammerData.release = objectPool.Release;
            
            hit = new BasicHitResolver(damageManager);
            EventSystem<bool>.Subscribe(EventType.PausedGame, PausedGame);
        }
        
        private void OnDisable()
        {
            inputManager.StartHit -= StartHit;
            upgradeManager.UpdateHit -= SetHit;
        }

        private void PausedGame(bool paused)
        {
            pausedGame = paused;
        }

        private void SetHit(IHitResolver newHit)
        {
            hit = newHit;
        }

        private void StartHit(Vector2 screenPos, Vector2 scaledScreenPos)
        {
            if (pausedGame)
                return;
            
            HammerData copy = hammerData;
            copy.baseHammerRotation = scaledScreenPos.x < 0.5 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);;
            copy.worldPos = GetWorldPos(screenPos);
            hit.ResolveHit(ref copy);
        }

        private Vector3 GetWorldPos(Vector2 screenPos)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPos);
        
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hammerSettings.groundLayerMask))
            {
                return hit.point;
            }
            Debug.LogError($"Hammer ray did not hit ground");
            return Vector3.zero;
        }
    }

}

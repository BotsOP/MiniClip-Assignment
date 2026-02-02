using Components.ObjectPool;
using Components.Player.Upgrades;
using UnityEngine;

namespace Components.Player
{
    public class HitManager : MonoBehaviour
    {
        [SerializeField] private HammerSettings hammerSettings;
        
        [Inject] private IInputManager inputManager;
        [Inject] private IUpgradeManager upgradeManager;
        [Inject] private ObjectPoolManager objectPool;

        private Transform hammerTransform;
        private Transform hammerPivot;
        private HammerData hammerData;
        private Camera mainCamera;
        private Vector2 cachedScaledStartScreenPos;
        private IHitResolver hit;
        
        private Quaternion BaseHammerRotation => cachedScaledStartScreenPos.x < 0.5 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        private void OnEnable()
        {
            inputManager.StartHit += StartHit;
            upgradeManager.UpdateHit += SetHit;
        
            mainCamera = Camera.main;
            hammerTransform = Instantiate(hammerSettings.hammerPrefab);
            hammerPivot = hammerTransform.GetChild(0);
            
            hammerData = hammerSettings.GetHammerData();
            hammerData.pivot = hammerPivot;
            hammerData.spawn = objectPool.Spawn;
            hammerData.release = objectPool.Release;
            
            hit = new BasicHitResolver();
        }
        
        private void OnDisable()
        {
            inputManager.StartHit -= StartHit;
            upgradeManager.UpdateHit -= SetHit;
        }

        private void SetHit(IHitResolver newHit)
        {
            hit = newHit;
        }

        private void StartHit(Vector2 screenPos, Vector2 scaledScreenPos)
        {
            Vector3 worldPos = GetWorldPos(screenPos);
            cachedScaledStartScreenPos = scaledScreenPos;
            hammerTransform.localRotation = BaseHammerRotation;
            hammerTransform.position = worldPos;
            hammerPivot.localRotation = Quaternion.Euler(0, 0, 90);

            HammerData copy = hammerData;
            copy.worldPos = worldPos;
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

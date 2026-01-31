using System.Collections.Generic;
using Components.Player.Upgrades;
using Managers;
using PrimeTween;
using UnityEngine;
using UnityEngine.UIElements;
using EventType = Managers.EventType;

namespace Components.Player
{
    public class HitManager : MonoBehaviour
    {
        [SerializeField] private HammerSettings hammerSettings;
        
        [Inject] private IInputManager inputManager;
        [Inject] private IUpgradeManager upgradeManager;

        private Transform hammerPivot;
        private HammerData hammerData;
        private Camera mainCamera;
        private Vector2 cachedScaledStartScreenPos;
        private IHitResolver hit;
        
        private Quaternion BaseHammerRotation => cachedScaledStartScreenPos.x < 0.5 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        private void OnEnable()
        {
            inputManager.StartHit += StartHit;
        
            mainCamera = Camera.main;
            hammerData = hammerSettings.GetHammerData();
            hammerSettings.hammerTransform = Instantiate(hammerSettings.hammerTransform);
            
            //THE FIRST CHILD NEEDS TO THE PIVOT OF THE HAMMER
            hammerPivot = hammerSettings.hammerTransform.GetChild(0);
            hammerData.pivot = hammerPivot;
            hit = new BasicHitResolver();
        }
        
        private void OnDisable()
        {
            inputManager.StartHit -= StartHit;
        }

        private void SetHit(IHitResolver newHit)
        {
            hit = newHit;
        }

        private void StartHit(Vector2 screenPos, Vector2 scaledScreenPos)
        {
            Vector3 worldPos = GetWorldPos(screenPos);
            cachedScaledStartScreenPos = scaledScreenPos;
            hammerSettings.hammerTransform.localRotation = BaseHammerRotation;
            hammerSettings.hammerTransform.position = worldPos;

            HammerData copy = hammerData;
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

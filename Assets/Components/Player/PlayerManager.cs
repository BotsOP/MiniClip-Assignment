using System.Collections.Generic;
using Managers;
using PrimeTween;
using UnityEngine;
using UnityEngine.UIElements;
using EventType = Managers.EventType;

namespace Components.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private HammerSettings hammerSettings;
        
        [Inject] private IInputManager inputManager;

        private Transform hammerPivot;
        private HammerData hammerData;
        private Camera mainCamera;
        private float hammerPower;
        private Vector2 cachedScaledStartScreenPos;
        private Vector3 cachedStartWorldPos;
        private List<IHitResolver> upgrades;
        
        private Quaternion BaseHammerRotation => cachedScaledStartScreenPos.x < 0.5 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        private void OnEnable()
        {
            inputManager.StartHit += StartHit;
            inputManager.PerformingHit += PerformingHit;
            inputManager.CancelledHit += CancelledHit;
        
            mainCamera = Camera.main;
            hammerData = hammerSettings.GetHammerData();
            hammerSettings.hammerTransform = Instantiate(hammerSettings.hammerTransform);
            
            //THE FIRST CHILD NEEDS TO THE PIVOT OF THE HAMMER
            hammerPivot = hammerSettings.hammerTransform.GetChild(0);
        }
        private void OnDisable()
        {
            inputManager.StartHit -= StartHit;
            inputManager.PerformingHit -= PerformingHit;
            inputManager.CancelledHit -= CancelledHit;
        }

        private void StartHit(Vector2 screenPos, Vector2 scaledScreenPos)
        {
            Vector3 worldPos = GetWorldPos(screenPos);
            cachedStartWorldPos = worldPos;
            cachedScaledStartScreenPos = scaledScreenPos;
            hammerSettings.hammerTransform.localRotation = BaseHammerRotation;
            hammerSettings.hammerTransform.position = worldPos;
        }
        private void PerformingHit(Vector2 screenPos, Vector2 scaledScreenPos)
        {
            float distance = Vector2.Distance(cachedScaledStartScreenPos, scaledScreenPos);
            hammerPower = distance * hammerSettings.hammerPowerDistanceScaling;
            hammerPower = Mathf.Clamp(hammerPower, 0, hammerSettings.maxHammerPower);

            hammerPivot.localRotation = Quaternion.Euler(0, 0, hammerPower * hammerSettings.hammerRotationScaling);
        }

        private void CancelledHit(Vector2 screenPos, Vector2 scaledScreenPos)
        {
            DamageInfo damageInfo = new DamageInfo(cachedStartWorldPos, hammerPower * hammerSettings.damageScaling);
            EventSystem<DamageInfo>.RaiseEvent(EventType.DoDamage, damageInfo);
            Sequence.Create(1, CycleMode.Restart, Ease.OutSine)
                .Group(Tween.LocalRotation(hammerPivot, Quaternion.identity, 0.1f));
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

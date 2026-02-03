using System;
using Components.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using static InputSystem_Actions;

public interface IInputManager
{
    event Action<Vector2, Vector2> StartHit;
    event Action<Vector2, Vector2> PerformingHit;
    event Action<Vector2, Vector2> CancelledHit;
}

public class InputActionsManager : MonoBehaviour, IPlayerActions, IInputManager, IDependencyProvider
{
    public event Action<Vector2, Vector2> StartHit;
    public event Action<Vector2, Vector2> PerformingHit;
    public event Action<Vector2, Vector2> CancelledHit;
    
    private InputSystem_Actions inputSystem;
    private bool isHitting;
    private Camera mainCamera;
        
    [Provide]
    public IInputManager ProvideInputManager()
    {
        Debug.Log("provided");
        return this;
    }
        
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        mainCamera = Camera.main;
        
        inputSystem = new InputSystem_Actions();
        inputSystem.Player.SetCallbacks(this);
        inputSystem.Enable();
    }

    private void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector2 scaledScreenPos = GetScaledScreenPos(screenPos);

        if (context.started)
        {
            Debug.Log($"start");
            isHitting = true;
            StartHit?.Invoke(screenPos, scaledScreenPos);
        }
        else if (context.performed)
        {
            Debug.Log($"erform");
            // isHitting = true;
            // StartHit?.Invoke(screenPos, scaledScreenPos);
        }
        else if (context.canceled)
        {
            Debug.Log($"end");
            isHitting = false;
            CancelledHit?.Invoke(screenPos, scaledScreenPos);
        }
    }
    public void OnTouchInput(InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector2 scaledScreenPos = GetScaledScreenPos(screenPos);

        if (context.started)
        {
            Debug.Log($"start");
            
        }
        else if (context.performed)
        {
            Debug.Log($"erform");
            isHitting = true;
            StartHit?.Invoke(screenPos, scaledScreenPos);
        }
        else if (context.canceled)
        {
            Debug.Log($"end");
            isHitting = false;
            CancelledHit?.Invoke(screenPos, scaledScreenPos);
        }
    }


    private void Update()
    {
        // if (!isHitting)
        //     return;
        
        Vector2 screenPos = GetScreenPos();
        Vector2 scaledScreenPos = GetScaledScreenPos(screenPos);
        PerformingHit?.Invoke(screenPos, scaledScreenPos);
    }
    
    private Vector2 GetScreenPos()
    {
        Vector2 hitPos = Vector2.zero;
        if (Mouse.current != null)
        {
            hitPos = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null)
        {
            hitPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        return hitPos;
    }

    private Vector2 GetScaledScreenPos(Vector2 screenPos)
    {
        float x = screenPos.x / mainCamera.pixelWidth;
        float y = screenPos.y / mainCamera.pixelHeight;
        return new Vector2(x, y);
    }
}


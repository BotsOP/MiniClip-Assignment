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
    private InputControl cachedDevice;
        
    [Provide]
    public IInputManager ProvideInputManager()
    {
        Debug.Log("provided");
        return this;
    }
        
    private void Awake()
    {
        mainCamera = Camera.main;
        
        inputSystem = new InputSystem_Actions();
        inputSystem.Player.SetCallbacks(this);
        inputSystem.Enable();
    }
    
    public void OnHit(InputAction.CallbackContext context)
    {
        cachedDevice = context.control.device;
        Vector2 screenPos = GetScreenPos(context.control.device);
        Vector2 scaledScreenPos = GetScaledScreenPos(screenPos);

        if (context.started)
        {
            isHitting = true;
            StartHit?.Invoke(screenPos, scaledScreenPos);
        }
        else if (context.canceled)
        {
            isHitting = false;
            CancelledHit?.Invoke(screenPos, scaledScreenPos);
        }
    }
    

    private void Update()
    {
        if (isHitting)
        {
            Vector2 screenPos = GetScreenPos(cachedDevice);
            Vector2 scaledScreenPos = GetScaledScreenPos(screenPos);
            PerformingHit?.Invoke(screenPos, scaledScreenPos);
        }
    }
    
    private Vector2 GetScreenPos(InputControl device)
    {
        Vector2 hitPos = Vector2.zero;
        if (device == Mouse.current)
        {
            hitPos = Mouse.current.position.ReadValue();
        }
        else if (device == Touchscreen.current)
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


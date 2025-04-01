using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
   
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnEndTouch;

    private InputSystem inputSystem;
    private Camera mainCamera;

    private void Awake()
    {
        inputSystem = new InputSystem();
        mainCamera = Camera.main;
        
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void Start()
    {
        inputSystem.Touch.TouchPress.started += ctx => StartTouch(ctx);
        inputSystem.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch started" + inputSystem.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnStartTouch != null)
        {
            OnStartTouch(Utils.ScreenToWorld(mainCamera, inputSystem.Touch.TouchPosition.ReadValue<Vector2>()),(float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        
        Debug.Log("Touch end at position: " + inputSystem.Touch.TouchPosition.ReadValue<Vector2>());
        if (OnEndTouch != null)
        {
            OnEndTouch(Utils.ScreenToWorld(mainCamera, inputSystem.Touch.TouchPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, inputSystem.Touch.TouchPosition.ReadValue<Vector2>());
    }


}

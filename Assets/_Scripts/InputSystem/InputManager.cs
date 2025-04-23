using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void StartTouchEvent(Vector2 vPosition, float fTime);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 vPosition, float fTime);
    public event StartTouchEvent OnEndTouch;
    #endregion

    #region Private Variables
    private InputSystem m_csInputSystem;
    private Camera m_camMainCamera;
    #endregion

    //—------Unity Events—----
    private void Awake()
    {
        Debug.Log("InputManager Awake");
        m_csInputSystem = new InputSystem();
        m_camMainCamera = Camera.main;

        Start();
    }

    private void OnEnable()
    {
        m_csInputSystem.Enable();
    }

    private void OnDisable()
    {
        m_csInputSystem.Disable();
    }

    private void Start()
    {
        m_csInputSystem.Touch.PrimaryContact.started += ctx => StartTouch(ctx);
        m_csInputSystem.Touch.PrimaryContact.canceled += ctx => EndTouch(ctx);
    }
    //—------------------

    //—------Private Functions—----
    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            Vector2 vPosition = Tools.ScreenToWorld(m_camMainCamera, m_csInputSystem.Touch.PrimaryPosition.ReadValue<Vector2>());
            OnStartTouch(vPosition, (float)context.startTime);
        }
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            Vector2 vPosition = Tools.ScreenToWorld(m_camMainCamera, m_csInputSystem.Touch.PrimaryPosition.ReadValue<Vector2>());
            OnEndTouch(vPosition, (float)context.startTime);
        }
    }
    //—------------------

    //—------public—----
    public Vector2 PrimaryPosition()
    {
        return Tools.ScreenToWorld(m_camMainCamera, m_csInputSystem.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
    //—------------------

    public Vector2 RawTouchPosition()
    {
        return m_csInputSystem.Touch.PrimaryPosition.ReadValue<Vector2>();
    }
}

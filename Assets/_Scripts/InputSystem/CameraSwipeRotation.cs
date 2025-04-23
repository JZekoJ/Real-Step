using UnityEngine;

public class CameraSwipeRotation : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float degreesPerPixel = 0.1f;

    private bool isTouching = false;
    private Vector2 startScreenPos;
    private float startAngle;

    private float currentAngle;

    private InputManager inputManager;
    private Camera mainCamera;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        mainCamera = Camera.main;
        currentAngle = transform.rotation.eulerAngles.y; // Angle de base au départ
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += OnStartTouch;
        inputManager.OnEndTouch += OnEndTouch;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= OnStartTouch;
        inputManager.OnEndTouch -= OnEndTouch;
    }

    private void OnStartTouch(Vector2 worldPos, float time)
    {
        startScreenPos = mainCamera.WorldToScreenPoint(worldPos);
        startAngle = currentAngle;
        isTouching = true;
    }

    private void OnEndTouch(Vector2 worldPos, float time)
    {
        isTouching = false;
    }

    private void Update()
    {
        if (isTouching)
        {
            Vector2 currentScreenPos = mainCamera.WorldToScreenPoint(inputManager.PrimaryPosition());
            float deltaX = currentScreenPos.x - startScreenPos.x;

            // Pas de calcul avec eulerAngles, on reste en float brut
            currentAngle = startAngle + deltaX * degreesPerPixel;

            transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }
    }
}

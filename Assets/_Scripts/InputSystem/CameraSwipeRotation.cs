using UnityEngine;

public class CameraSwipeRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.2f;

    private bool isSwiping = false;
    private Vector2 previousTouchPosition;

    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += HandleStartTouch;
        InputManager.Instance.OnEndTouch += HandleEndTouch;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnStartTouch -= HandleStartTouch;
        InputManager.Instance.OnEndTouch -= HandleEndTouch;
    }

    private void Update()
    {
        if (isSwiping)
        {
            // ! On récupère la position RAW de l'écran (pas convertie en World)
            Vector2 currentPosition = InputManager.Instance.RawTouchPosition();
            float deltaX = currentPosition.x - previousTouchPosition.x;

            float horizontalRotation = deltaX * rotationSpeed;
            transform.Rotate(0f, horizontalRotation, 0f);

            previousTouchPosition = currentPosition;
        }
    }

    private void HandleStartTouch(Vector2 position, float time)
    {
        previousTouchPosition = InputManager.Instance.RawTouchPosition();
        isSwiping = true;
    }

    private void HandleEndTouch(Vector2 position, float time)
    {
        isSwiping = false;
    }
}

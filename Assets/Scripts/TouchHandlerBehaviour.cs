using UnityEngine;
using UnityEngine.InputSystem;

public class TouchHandlerBehaviour : MonoBehaviour
{
    private BallBehaviour _ball;
    private Vector2 _lastTouchPosition;
    private InputAction _touchEndAction;
    private Vector2 _touchEndPosition;
    private float _touchEndTime;
    private TouchInputActions _touchInputActions;
    private InputAction _touchPositionAction;
    private InputAction _touchStartAction;
    private Vector2 _touchStartPosition;
    private float _touchStartTime;

    private void Awake()
    {
        _ball = GetComponent<BallBehaviour>();
        _touchInputActions = new TouchInputActions();
        _touchPositionAction = _touchInputActions.TouchControl.TouchPosition;
        _touchStartAction = _touchInputActions.TouchControl.TouchStart;
        _touchEndAction = _touchInputActions.TouchControl.TouchEnd;
    }

    private void OnEnable()
    {
        _touchPositionAction.Enable();
        _touchStartAction.Enable();
        _touchEndAction.Enable();
        _touchPositionAction.performed += OnTouchPosition;
        _touchStartAction.performed += OnTouchStart;
        _touchEndAction.performed += OnTouchEnd;
    }

    private void OnDisable()
    {
        _touchPositionAction.Disable();
        _touchStartAction.Disable();
        _touchEndAction.Disable();
        _touchPositionAction.performed -= OnTouchPosition;
        _touchStartAction.performed -= OnTouchStart;
        _touchEndAction.performed -= OnTouchEnd;
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        _lastTouchPosition = context.ReadValue<Vector2>();
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        _touchStartPosition = _lastTouchPosition;
        _touchStartTime = Time.time;
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        _touchEndPosition = _lastTouchPosition;
        _touchEndTime = Time.time;

        _ball.Throw(_touchStartPosition, _touchEndPosition, _touchEndTime - _touchStartTime);
    }
}
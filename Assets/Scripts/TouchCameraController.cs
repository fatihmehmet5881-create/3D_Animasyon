using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchCameraController : MonoBehaviour
{
    [SerializeField] private Transform PlayerBody;
    [SerializeField] private Transform CameraBody;
    [SerializeField] private float TouchSensivity = 0.15f;
    [SerializeField] private float UpperLookLimit = 80f;
    [SerializeField] private float LowerLookLimit = -80f;

    private float cameraVerticalRotation = 0f;
    private int cameraFingerID = -1;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void Osable()
    {
        EnhancedTouchSupport.Disable();
    }
    private void Update()
    {
        if(Touchscreen.current != null) HandleTouchScreenLook();

#if UNITY_EDITOR
        HandleMouseLook();
#endif
    }

    private void HandleTouchScreenLook()
    {
        var activeTouches = Touch.activeTouches;
        if(activeTouches.Count == 0)
        {
            cameraFingerID = -1;
            return;
        }
        foreach(var touch in activeTouches)
        {
            if(touch.phase == UnityEngine.InputSystem.TouchPhase.Began) {
                if(touch.screenPosition.x > (Screen.width / 2)&& !EventSystem.current.IsPointerOverGameObject(touch.touchId))
                    if(cameraFingerID == -1) cameraFingerID = touch.touchId;
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                if(touch.touchId == cameraFingerID) RotateView(touch.delta.x,touch.delta.y);
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                if (touch.touchId == cameraFingerID) cameraFingerID = -1;
        }
    }
    private void HandleMouseLook()
    {
        if(Mouse.current == null) return;
        if(Mouse.current.leftButton.isPressed && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 MouseDelta = Mouse.current.delta.ReadValue();
            RotateView(MouseDelta.x, MouseDelta.y);
        }
    }
    private void RotateView(float deltaX, float deltaY)
    {
        if(PlayerBody != null) PlayerBody.Rotate(Vector3.up * deltaX * TouchSensivity);
        cameraVerticalRotation -= deltaY * TouchSensivity;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, LowerLookLimit, UpperLookLimit);
        if(CameraBody != null) CameraBody.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
}
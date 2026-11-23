using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    void Start()
    {
        if(cameras != null && 0 < cameras.Length) SwitchCamera(0);
    }
    void Update()
    {
        if(Keyboard.current == null) return;

        if(Keyboard.current.digit1Key.wasPressedThisFrame) SwitchCamera(0);
        else if(Keyboard.current.digit2Key.wasPressedThisFrame) SwitchCamera(1);
        else if(Keyboard.current.digit3Key.wasPressedThisFrame) SwitchCamera(2);
        else if(Keyboard.current.digit4Key.wasPressedThisFrame) SwitchCamera(3);
        else if(Keyboard.current.digit5Key.wasPressedThisFrame) SwitchCamera(4);
    }

    void SwitchCamera(int x)
    {
        if(cameras == null || x < 0 || cameras.Length <= x) return;
        for(int i = 0; i < cameras.Length; i++) cameras[i].gameObject.SetActive(i == x);
    }
}

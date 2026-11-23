using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcherController : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras;
    private int cameraView = 0;

    void Start()
    {
        ActiveCameras(0);
    }
    void Update()
    {
        if(Keyboard.current == null) return;

        int maxCheck = Mathf.Min(cameras.Count, 9);
        for(int i = 0; i < maxCheck; i++)
        {
            Key TargetKey = Key.Digit1 + i;
            if(Keyboard.current[TargetKey].wasPressedThisFrame)
            {
                ActiveCameras(i);
                break;
            }
        }
    }

    private void ActiveCameras(int x)
    {
        if(x < 0 || cameras.Count <= 0) return;
        
        for(int i = 0; i < cameras.Count; i++) if(cameras[i] != null) cameras[i].gameObject.SetActive(i == x);
    }

    public void CameraSwitch()
    {
        cameraView++;
        if(cameraView == 2) cameraView = 0;
        ActiveCameras(cameraView);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GunRaycastController : MonoBehaviour
{
    [SerializeField] private float range = 100f;
    [SerializeField] private float impactForce = 300f;
    [SerializeField] private int damage = 25;
    [SerializeField] private LayerMask hitLayers;

    [Header("First Person Camera")]
    [SerializeField] private Camera fpsCamera; 

    private void Awake()
    {
        if (fpsCamera == null) fpsCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (IsPointerOverUI()) return;
            
            Shoot();
        }
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            if (IsPointerOverUI()) return;

            Shoot();
        }
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        if (Mouse.current != null && EventSystem.current.IsPointerOverGameObject()) return true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            if (EventSystem.current.IsPointerOverGameObject(touchId)) return true;
        }
        return false;
    }
    private void Shoot()
    {
        if (fpsCamera == null) return;

        RaycastHit hit;

        Vector3 rayOrigin = fpsCamera.transform.position;
        Vector3 shootDirection = fpsCamera.transform.forward;

        Debug.DrawRay(rayOrigin, shootDirection * range, Color.green, 1f);

        if (Physics.Raycast(rayOrigin, shootDirection, out hit, range, hitLayers))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb != null)
            {
                Vector3 forceDirection = shootDirection * impactForce;
                rb.AddForceAtPosition(forceDirection, hit.point, ForceMode.Impulse);
            }
            
            Target target = hit.collider.GetComponent<Target>();
            if (target != null) target.TakeDamage(damage);
        }
    }
}
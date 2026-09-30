using UnityEngine;
using UnityEngine.InputSystem;

public class ClickDetector : MonoBehaviour
{
    public Camera cam;
    public LayerMask clickableLayer;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Click detected");
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, clickableLayer))
            {
                Debug.Log("Hit: " + hit.collider.name);
                var clickable = hit.collider.GetComponentInParent<Clickable>();
                clickable?.OnClicked();
            }
        }
    }
}
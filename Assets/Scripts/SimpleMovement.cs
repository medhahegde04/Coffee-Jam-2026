using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMovement : MonoBehaviour
{
    public float maxYaw = 25f;
    public float sensitivity = 0.05f;
    Quaternion startRotation;

    void Start() => startRotation = transform.localRotation;

    void Update()
    {
        if (Mouse.current == null) return;
        float mouseX = Mouse.current.position.ReadValue().x - (Screen.width / 2f);
        float yaw = Mathf.Clamp(mouseX * sensitivity, -maxYaw, maxYaw);
        transform.localRotation = startRotation * Quaternion.Euler(0, yaw, 0);
    }
}
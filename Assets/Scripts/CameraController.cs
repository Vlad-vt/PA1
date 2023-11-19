using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Target object for the camera to look at
    public Transform target;

    // Rotation speed of the camera
    public float rotationSpeed = 1.0f;

    // Zoom speed of the camera
    public float zoomSpeed = 2.0f;

    // Minimum zoom distance allowed
    public float minZoomDistance = 1.0f;

    // Maximum zoom distance allowed
    public float maxZoomDistance = 10.0f;

    // Last recorded mouse position for calculating mouse delta
    private Vector3 lastMousePosition;

    // Current distance of the camera from the target
    private float distance = 5.0f;

    void Start()
    {
        // Ensure the target is not null, and make the camera initially look at it
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    void Update()
    {
        // Check if there is a target assigned
        if (target == null)
        {
            return;
        }

        // Right mouse button pressed, record the initial mouse position
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        // Right mouse button held down, rotate the camera based on mouse movement
        if (Input.GetMouseButton(1))
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;

            float horizontalInput = deltaMouse.x * rotationSpeed;
            float verticalInput = deltaMouse.y * rotationSpeed;

            // Rotate the camera around the target based on mouse movement
            transform.RotateAround(target.position, Vector3.up, horizontalInput);
            transform.RotateAround(target.position, transform.right, -verticalInput);

            lastMousePosition = Input.mousePosition;
        }

        // Zoom input from the mouse scroll wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the zoom distance based on the input, clamped within specified bounds
        distance -= zoomInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);

        // Update the camera position along the Z-axis based on the new distance
        Vector3 offset = transform.rotation * Vector3.forward * -distance;
        transform.position = target.position + offset;
    }
}

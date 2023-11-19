using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1.0f;
    public float zoomSpeed = 2.0f;
    public float minZoomDistance = 1.0f;
    public float maxZoomDistance = 10.0f;

    private Vector3 lastMousePosition;
    private float distance = 5.0f; 

    void Start()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1)) 
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;

            float horizontalInput = deltaMouse.x * rotationSpeed;
            float verticalInput = deltaMouse.y * rotationSpeed;

            transform.RotateAround(target.position, Vector3.up, horizontalInput);
            transform.RotateAround(target.position, transform.right, -verticalInput);

            lastMousePosition = Input.mousePosition;
        }

        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        distance -= zoomInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);

        // Обновление позиции камеры по Z-координате
        Vector3 offset = transform.rotation * Vector3.forward * -distance;
        transform.position = target.position + offset;
    }


}

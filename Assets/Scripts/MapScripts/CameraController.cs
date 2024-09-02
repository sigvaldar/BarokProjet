using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float zoomSpeed = 10f;
    public float minZoom = 10f;
    public float maxZoom = 100f;

    void Update()
    {
        // Rotation
        if (Input.GetMouseButton(1)) // Clic droit pour tourner
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, horizontal, 0);
        }

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);
    }
}

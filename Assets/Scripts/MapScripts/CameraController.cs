using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float zoomSpeed = 10f;
    public float CameraSpeed = 0.6f;
    public float minZoom = 10f;
    public float maxZoom = 100f;

    void Update()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
        {
            MouveCamera(Vector3.forward);
        }
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            MouveCamera(Vector3.left);
        }
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            MouveCamera(Vector3.back);
        }
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            MouveCamera(Vector3.right);
        }

        // Rotation
        if (Input.GetMouseButton(1)) // Clic droit pour tourner
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            transform.Rotate(vertical, horizontal, 0);
        }

        CameraZoom();
    }

    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);
    }

    public void MouveCamera(Vector3 direction)
    {
        Camera.main.transform.position += direction * Time.deltaTime * CameraSpeed;
    }
}

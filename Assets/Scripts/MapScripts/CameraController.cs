using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float zoomSpeed = 10f;
    public float CameraSpeed = 1f;
    public float minZoom = 10f;
    public float maxZoom = 100f;


    private int charger = 0;
    [SerializeField] Vector3 ZoomOutFull;
    [SerializeField] Image chargeBar;
    private bool fullView;

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
            CameraRotation();
        }

        CameraZoom();
    }

    private void CameraRotation()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime * -1f;
        transform.Rotate(vertical, horizontal, 0);
    }

    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (!fullView)
        {
            Camera.main.fieldOfView -= scroll * zoomSpeed;
            if ((Camera.main.fieldOfView >= maxZoom) & (Input.GetAxis("Mouse ScrollWheel") < 0))
            {
                charger += 5;
                chargeBar.transform.DOScaleX(chargeBar.transform.localScale.x + 2, 1);
                //Camera.main.transform.DOMove();
            }
            else if ((Camera.main.fieldOfView >= maxZoom) & (Input.GetAxis("Mouse ScrollWheel") > 0))
            {
                charger -= 5;
                chargeBar.transform.DOScaleX(chargeBar.transform.localScale.x - 2, 1);
            }
            else if ((Camera.main.fieldOfView < maxZoom) & ((Input.GetAxis("Mouse ScrollWheel") > 0)))
            {
                charger = 0;
                chargeBar.transform.DOScaleX(0, 1);
            }
            if (charger > 100)
            {
                Camera.main.transform.DOMove(ZoomOutFull, 2f);
                charger = 0;
                chargeBar.transform.DOScaleX(0, 1);
                fullView = true;
            }
        }
        else if (fullView)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) 
            {
                if (chargeBar.transform.localScale.x <= 0)
                {
                    chargeBar.transform.localScale = new Vector3(5,1,1);
                }
                charger += 5;
                chargeBar.transform.DOScaleX(chargeBar.transform.localScale.x - 1, 1);
            }
            if (charger > 100)
            {
                Vector3 newpos = new Vector3(Camera.main.transform.position.x, 20, Camera.main.transform.position.z);
                Camera.main.transform.DOMove(newpos,2);
                chargeBar.transform.DOScaleX(0, 1);
                charger = 0;
                fullView = false;
            }
        }
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);

    }

    public void MouveCamera(Vector3 direction)
    {
        float modificator = Camera.main.fieldOfView/100;
        Camera.main.transform.position += direction * Time.deltaTime * CameraSpeed;
    }
}

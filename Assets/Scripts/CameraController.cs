using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera speed
    public float movementSpeed;
    public float movementTime;

    //Camera bounds
    public float ogXCamBound;
    public float ogYCamBound;
    public float xCamBound;
    public float yCamBound;

    //Camera zoom interval and bounds
    public Vector3 zoomAmount;
    public float zoomMin;
    public float zoomMax;

    public Camera mainCam;

    //Camera focus
    public bool cameraLocked;
    public MovingObject target;

    public Vector3 newZoom;
    public Vector3 newPosition;
    public Vector3 dragStart;
    public Vector3 dragCurrent;

    // Start is called before the first frame update
    void Start()
    {
        newPosition.x = transform.position.x;
        newPosition.y = transform.position.y;
        newPosition.z = transform.position.z;
    }

    public void BattleCamera(Board board)
    {
        newPosition.x = board.columns / 2;
        newPosition.y = board.rows / 2;

        xCamBound = board.columns;
        yCamBound = board.rows;
    }

    public void ResetCamera()
    {
        newPosition.x = ogXCamBound / 2;
        newPosition.y = ogYCamBound / 2;

        xCamBound = ogXCamBound;
        yCamBound = ogYCamBound;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMovement();
        if (target != null && cameraLocked)
        {
            CameraLock();
        }
    }

    void HandleCameraMovement()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
            newZoom.z = Mathf.Clamp(newZoom.z, zoomMin, zoomMax);
        }

        if (Input.GetMouseButtonDown(2))
        {
            cameraLocked = false;

            Plane plane = new Plane(Vector3.forward, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStart = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.forward, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrent = ray.GetPoint(entry);

                newPosition = transform.position + dragStart - dragCurrent;
                newPosition.x = Mathf.Clamp(newPosition.x, 0, xCamBound);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, yCamBound);
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    public void CameraLookAt(MovingObject target)
    {
        cameraLocked = true;
        this.target = target;
        newPosition.x = Mathf.Clamp(target.transform.position.x, 0, xCamBound);
        newPosition.y = Mathf.Clamp(target.transform.position.y, 0, yCamBound);
    }

    public void CameraLock()
    {
        newPosition.x = Mathf.Clamp(target.transform.position.x, 0, xCamBound);
        newPosition.y = Mathf.Clamp(target.transform.position.y, 0, yCamBound);
    }
}


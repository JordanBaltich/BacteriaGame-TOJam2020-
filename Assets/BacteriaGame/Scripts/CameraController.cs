using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;

    public float minCameraZoomY;
    public float maxCameraZoomY;
    public float minCameraZoomZ;
    public float maxCameraZoomZ;


    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;



    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newZoom = cameraTransform.localPosition;
       

    }

    // Update is called once per frame
    void Update()
    {
     
        MovementInput();
        MouseInput();
    }


    void MouseInput()
    {
        //mouse scrolling for camera zoom
        if(Input.mouseScrollDelta.y != 0)
        {

            newZoom += Input.mouseScrollDelta.y * zoomAmount;

        }

        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray,out entry))
            {

                dragStartPosition = ray.GetPoint(entry);
            }

        }

        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {

                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }

        }


    }
    void MovementInput()
    {
        // for faster camera movement if holding down left shift.
        if (Input.GetKey(KeyCode.LeftShift))
        {

            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }


        // WASD and Arrow key camera movement.
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        //keyboard inputs for camera zoom

        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            newZoom -= zoomAmount;
        }

        //clamping the camera zoom NOT CURRENTLY WORKING AT THE MOMENT HENCE WHY ITS COMMENTED OUT.

        //cameraTransform.position.y = Mathf.Clamp(cameraTransform.localPosition.y, minCameraZoomY, maxCameraZoomY);
        //cameraTransform.position.z = Mathf.Clamp(cameraTransform.localPosition.z, minCameraZoomZ, maxCameraZoomZ);

        


        //used for smooth camera movement
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

      
    }



}

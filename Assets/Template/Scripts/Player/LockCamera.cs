using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] new Camera camera;
    [SerializeField] float cameraHorizontalRotationSpeed = 30;
    [SerializeField] float cameraVerticalRotationSpeed = 5;
    [SerializeField] float verticalMinAngle = 20;
    [SerializeField] float verticalMaxAngle = 50;
    [SerializeField] float cameraZoomSpeed = 1;
    [SerializeField] float[] cameraZoomLevels;
    //[SerializeField] float smoothFactorCameraRotationSpeed;
    private Vector2 inputValueRotation = Vector2.zero;
    private int currentZoomIndex = 0;
    //private Vector3 offset;

    [Range(0.01f, 1.0f)]
    [SerializeField] float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position - player.position;
        currentZoomIndex = cameraZoomLevels.Length / 2;
        camera.orthographicSize = cameraZoomLevels[currentZoomIndex];
    }

    private void FixedUpdate()
    {
        //Vector3 newPos = player.position + offset;
        transform.position = Vector3.Slerp(transform.position, player.position, smoothFactor);

        if(transform.rotation.eulerAngles.x >= verticalMaxAngle)
        {
            if(inputValueRotation.y > 0)
            {
                inputValueRotation.y = 0;
            }
        }
        if (transform.rotation.eulerAngles.x <= verticalMinAngle)
        {
            if (inputValueRotation.y < 0)
            {
                inputValueRotation.y = 0;
            }
        }
        transform.Rotate(0, inputValueRotation.x * cameraHorizontalRotationSpeed * Time.fixedDeltaTime, 0, Space.World);
        transform.Rotate(inputValueRotation.y * cameraVerticalRotationSpeed * Time.fixedDeltaTime, 0, 0, Space.Self);

        /*if(cameraDistance.position.z <= minCameraZoom)
        {
            if (inputValueZoom > 0)
            {
                inputValueZoom = 0;
            }
        }
        if (cameraDistance.position.z >= maxCameraZoom)
        {
            if (inputValueZoom < 0)
            {
                inputValueZoom = 0;
            }
        }*/
        //cameraDistance.orthographicSize += inputValueZoom * cameraZoomSpeed * Time.fixedDeltaTime;
    }

    void OnCameraMovement(InputValue value)
    {
        inputValueRotation = value.Get<Vector2>();
    }

    void OnCameraZoom(InputValue value)
    {
        if (value.isPressed)
        {
            currentZoomIndex++;
            if(currentZoomIndex >= cameraZoomLevels.Length)
            {
                currentZoomIndex = 0;
            }
            camera.orthographicSize = cameraZoomLevels[currentZoomIndex];
        }
    }
}

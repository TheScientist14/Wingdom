using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private new Camera camera;
    [SerializeField] private float cameraHorizontalRotationSpeed = 30;
    [SerializeField] private float cameraVerticalRotationSpeed = 5;
    [SerializeField] private float verticalMinAngle = 20;
    [SerializeField] private float verticalMaxAngle = 50;
    //[SerializeField] float cameraZoomSpeed = 1;
    [SerializeField] private float[] cameraZoomLevels;
    //[SerializeField] float smoothFactorCameraRotationSpeed;
    private Vector2 _inputValueRotation = Vector2.zero;
    private int _currentZoomIndex = 0;
    //private Vector3 offset;

    [Range(0.01f, 1.0f)]
    [SerializeField]
    private float smoothFactor = 0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        //offset = transform.position - player.position;
        _currentZoomIndex = cameraZoomLevels.Length / 2;
        camera.orthographicSize = cameraZoomLevels[_currentZoomIndex];
    }

    private void FixedUpdate()
    {
        //Vector3 newPos = player.position + offset;
        transform.position = Vector3.Slerp(transform.position, player.position, smoothFactor);

        if(transform.rotation.eulerAngles.x >= verticalMaxAngle)
        {
            if(_inputValueRotation.y > 0)
            {
                _inputValueRotation.y = 0;
            }
        }
        if (transform.rotation.eulerAngles.x <= verticalMinAngle)
        {
            if (_inputValueRotation.y < 0)
            {
                _inputValueRotation.y = 0;
            }
        }
        transform.Rotate(0, _inputValueRotation.x * cameraHorizontalRotationSpeed * Time.fixedDeltaTime, 0, Space.World);
        transform.Rotate(_inputValueRotation.y * cameraVerticalRotationSpeed * Time.fixedDeltaTime, 0, 0, Space.Self);

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

    private void OnCameraMovement(InputValue value)
    {
        _inputValueRotation = value.Get<Vector2>();
    }

    private void OnCameraZoom(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("cc");
            _currentZoomIndex++;
            if(_currentZoomIndex >= cameraZoomLevels.Length)
            {
                _currentZoomIndex = 0;
            }
            camera.orthographicSize = cameraZoomLevels[_currentZoomIndex];
        }
    }
}

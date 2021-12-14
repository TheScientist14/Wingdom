using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float cameraRotationSpeed = 1;
    //[SerializeField] float smoothFactorCameraRotationSpeed;
    private float inputValue = 0;
    //private Vector3 offset;

    [Range(0.01f, 1.0f)]
    [SerializeField] float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position - player.position;
    }

    private void FixedUpdate()
    {
        //Vector3 newPos = player.position + offset;
        transform.position = Vector3.Slerp(transform.position, player.position, smoothFactor);
        transform.Rotate(0, inputValue * cameraRotationSpeed * Time.fixedDeltaTime, 0, Space.World);
    }

    void OnCameraRight(InputValue value)
    {
        inputValue = value.Get<float>();
    }
}

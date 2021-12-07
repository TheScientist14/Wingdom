using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    private Vector3 movement = Vector3.zero;
    private Rigidbody rb;

    private float speed = 5;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (movement * speed * Time.fixedDeltaTime));
    }

    void OnRight(InputValue value)
    {
        movement = new Vector3(0, 0, movement.z) + value.Get<float>() * Vector3.right;
    }

    void OnForward(InputValue value)
    {
        movement = new Vector3(movement.x, 0, 0) + value.Get<float>() * Vector3.forward;
    }
}

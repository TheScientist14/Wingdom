using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    // character
    private Rigidbody rb;
    private CapsuleCollider col;
    private new Camera camera;

    // movement
    [SerializeField] float speed = 5;
    private Vector3 movement = Vector3.zero;
    private float lastForwardInput = 0;
    private float lastRightInput = 0;
    [SerializeField] float jumpForce = 2;
    [SerializeField] float isGroundedTolerance = 0.2f;
    private bool isGrounded = true;
    private bool jump = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        rb.velocity = movement * speed /** Time.fixedDeltaTime*/ + new Vector3(0, rb.velocity.y, 0);
        if (jump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

/*    Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.position - 10 * transform.up, out hit);
        return hit.normal;
    }*/

    void OnRight(InputValue value)
    {
        float val = value.Get<float>();
        movement = movement + (val - lastRightInput) * transform.right;
        lastRightInput = val;
    }

    void OnForward(InputValue value)
    {
        float val = value.Get<float>();
        movement = movement + (val - lastForwardInput) * transform.forward;
        lastForwardInput = val;
    }

    void OnJump()
    {
        if (isGrounded)
        {
            jump = true;
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision colInfo)
    {
        Debug.Log("collision");
        foreach(ContactPoint contactPoint in colInfo.contacts)
        {
            if (Vector3.Dot(transform.up, contactPoint.normal) <= isGroundedTolerance)
            {
                isGrounded = true;
            }
        }
    }
}

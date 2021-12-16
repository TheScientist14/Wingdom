using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    // character
    private Rigidbody rb;
    //private CharacterController characterController;
    private CapsuleCollider col;
    //private MeshRenderer meshRenderer;

    private new Camera camera;

    // movement
    [SerializeField] float speed = 5;
    private Vector2 movement = Vector2.zero;
    [SerializeField] float jumpForce = 2;
    [SerializeField] float flatTerrainTolerance = 0.2f;
    [SerializeField] float airTolerance = 0.01f;
    private bool isGrounded = true;
    private bool jump = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();
        col = GetComponent<CapsuleCollider>();
        //meshRenderer = GetComponent<MeshRenderer>();
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
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = camera.transform.right;
        right.y = 0;
        right.Normalize();
        Vector3 mov = Vector3.zero;
        Vector3 groundNormal = GetGroundNormal();
/*        if(groundNormal != null && groundNormal.magnitude != 0)
        {
            meshRenderer.material.color = Color.blue;
        }
        else
        {
            meshRenderer.material.color = Color.red;
        }*/
        if(movement.magnitude >= 0.01)
        {
            // wants to move
            mov = movement.x * right + movement.y * forward;
            if (groundNormal != null && groundNormal.magnitude != 0)
            {
                // grounded
                mov = mov.x * NormalToRight(groundNormal) + mov.z * NormalToForward(groundNormal);
                rb.position += transform.up * mov.y * Time.fixedDeltaTime;
                mov.y = 0;
                rb.velocity = (mov * speed); //+ rb.velocity.y * Vector3.up;
            }
            else
            {
                // in the air
                rb.velocity = new Vector3(mov.x * speed, rb.velocity.y, mov.z * speed);
            }
            transform.LookAt(transform.position + mov);
        }
        else
        {
            // doesn�t want to move
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 10), rb.velocity.z);
        if (jump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }

    Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        /*if (Physics.Raycast(transform.position - (col.height/2) * transform.up, -transform.up, out hit, 0.2f))
        {
            return hit.normal;
        }
        else
        {
            Debug.Log("air !!!");
            return Vector3.zero;
        }*/
        if (Physics.SphereCast(transform.position - (col.height / 2 - col.radius - airTolerance / 2) * transform.up, col.radius, -transform.up, out hit, 1))
        {
            if(hit.distance < airTolerance)
            {
                return hit.normal;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            Debug.Log("air !!!");
            return Vector3.zero;
        }
        //return Vector3.up;
    }

    Vector3 NormalToRight(Vector3 normal)
    {
        if(normal.y == 0)
        {
            if(normal.x != 0)
            {
                // normal = (x, 0, z), x != 0
                return Vector3.zero;
            }
            else
            {
                // normal = (0, 0, z), z != 0
                return Vector3.right;
            }
        }
        else
        {
            float y = - normal.x / normal.y;
            return (new Vector3(1, y, 0).normalized);
        }
    }

    Vector3 NormalToForward(Vector3 normal)
    {
        if (normal.y == 0)
        {
            if(normal.z != 0)
            {
                // normal = (x, 0, z), z != 0
                return Vector3.zero;
            }
            else
            {
                // normal = (x, 0, 0), x != 0
                return Vector3.forward;
            }
        }
        else
        {
            // normal = (x, y, z), y != 0
            float y = -normal.z / normal.y;
            return (new Vector3(0, y, 1).normalized);
        }
    }

    void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
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
        foreach(ContactPoint contactPoint in colInfo.contacts)
        {
            if (Vector3.Dot(transform.up, contactPoint.normal.normalized) >= 1 - flatTerrainTolerance)
            {
                isGrounded = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        col = GetComponent<CapsuleCollider>();
        Vector3 origin = transform.position - (col.height / 2 - col.radius - airTolerance / 2) * transform.up;
        Gizmos.DrawSphere(origin, 0.05f);
        Ray ray = new Ray(origin, -transform.up);
        Gizmos.DrawRay(ray);
        RaycastHit hit;
        if (Physics.SphereCast(ray, col.radius, out hit, 1))
        {
            if(hit.distance < airTolerance)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hit.point, hit.point + 2 * NormalToForward(hit.normal));
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hit.point, hit.point + 2 * NormalToRight(hit.normal));
            }
        }
    }
}

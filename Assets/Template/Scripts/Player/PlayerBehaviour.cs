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
    [SerializeField] float speedUpSquaredThreshold = 0.25f;
    private Vector2 movement = Vector2.zero;
    [SerializeField] float jumpForce = 2;
    [SerializeField] float flatTerrainTolerance = 0.2f;
    [SerializeField] float airTolerance = 0.01f;
    private bool isGrounded = true;
    private bool jump = false;
    private bool hasSpeedUp = false;

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
        if(movement.magnitude >= 0.01)
        {
            // wants to move
            Vector3 right = camera.transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            Vector3 mov = movement.x * right + movement.y * forward;

            // getting ground info
            RaycastHit hit;
            isGrounded = false;
            if (Physics.SphereCast(transform.position - (col.height / 2 - col.radius - airTolerance / 2) * transform.up, col.radius, -transform.up, out hit, 1))
            {
                if (hit.distance <= airTolerance)
                {
                    isGrounded = true;
                }
            }

            if (isGrounded && hit.normal != null && hit.normal.magnitude != 0)
            {
                // grounded
                mov = mov.x * NormalToRight(hit.normal) + mov.z * NormalToForward(hit.normal);
                rb.position -= hit.distance * transform.up;
                rb.position += mov.y * speed * Time.fixedDeltaTime * transform.up;
                mov.y = 0;
                rb.velocity = (mov * speed);
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
            // doesn’t want to move
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        /*rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 10), rb.velocity.z);*/
        if (jump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
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
        if(movement.sqrMagnitude > speedUpSquaredThreshold)
        {
            if (!hasSpeedUp)
            {
                if (!IsInvoking(nameof(SpeedUp)))
                {
                    Invoke(nameof(SpeedUp), 2);
                }
            }
        }
        else
        {
            if (hasSpeedUp)
            {
                CancelInvoke(nameof(SpeedUp));
                hasSpeedUp = false;
                speed /= 2;
            }
        }
    }

    void SpeedUp()
    {
        if (!hasSpeedUp)
        {
            hasSpeedUp = true;
            speed *= 2;
        }
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

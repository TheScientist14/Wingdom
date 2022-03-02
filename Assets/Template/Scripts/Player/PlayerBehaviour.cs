using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    // character
    private Rigidbody _rb;
    private CapsuleCollider _col;

    private Camera _camera;

    // movement
    [SerializeField] float speed = 5;
    [SerializeField] float speedUpSquaredThreshold = 0.25f;
    private Vector2 _movement = Vector2.zero;
    [SerializeField] float jumpForce = 2;
    [SerializeField] float flatTerrainTolerance = 0.2f;
    [SerializeField] float airTolerance = 0.01f;
    private bool _isGrounded = true;
    private bool _jump = false;
    private bool _hasSpeedUp = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_movement.magnitude >= 0.01)
        {
            // wants to move
            Vector3 right = _camera.transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);
            Vector3 mov = _movement.x * right + _movement.y * forward;

            // getting ground info
            RaycastHit hit;
            _isGrounded = false;
            if (Physics.SphereCast(transform.position - (_col.height / 2 - _col.radius - airTolerance / 2) * transform.up, _col.radius, -transform.up, out hit, 1))
            {
                if (hit.distance <= airTolerance)
                {
                    _isGrounded = true;
                }
            }

            if (_isGrounded && hit.normal != null && hit.normal.magnitude != 0)
            {
                // grounded
                mov = mov.x * NormalToRight(hit.normal) + mov.z * NormalToForward(hit.normal);
                _rb.position -= hit.distance * transform.up;
                _rb.position += mov.y * speed * Time.fixedDeltaTime * transform.up;
                mov.y = 0;
                _rb.velocity = (mov * speed);
            }
            else
            {
                // in the air
                _rb.velocity = new Vector3(mov.x * speed, _rb.velocity.y, mov.z * speed);
            }
            transform.LookAt(transform.position + mov);
        }
        else
        {
            // doesn’t want to move
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
        /*rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10, 10), rb.velocity.z);*/
        if (_jump)
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            _jump = false;
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
        _movement = value.Get<Vector2>();
        if(_movement.sqrMagnitude > speedUpSquaredThreshold)
        {
            if (!_hasSpeedUp)
            {
                if (!IsInvoking(nameof(SpeedUp)))
                {
                    Invoke(nameof(SpeedUp), 2);
                }
            }
        }
        else
        {
            if (_hasSpeedUp)
            {
                CancelInvoke(nameof(SpeedUp));
                _hasSpeedUp = false;
                speed /= 2;
            }
        }
    }

    void SpeedUp()
    {
        if (!_hasSpeedUp)
        {
            _hasSpeedUp = true;
            speed *= 2;
        }
    }

    void OnJump()
    {
        if (_isGrounded)
        {
            _jump = true;
            _isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision colInfo)
    {
        foreach(ContactPoint contactPoint in colInfo.contacts)
        {
            if (Vector3.Dot(transform.up, contactPoint.normal.normalized) >= 1 - flatTerrainTolerance)
            {
                _isGrounded = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        _col = GetComponent<CapsuleCollider>();
        Vector3 origin = transform.position - (_col.height / 2 - _col.radius - airTolerance / 2) * transform.up;
        Gizmos.DrawSphere(origin, 0.05f);
        Ray ray = new Ray(origin, -transform.up);
        Gizmos.DrawRay(ray);
        RaycastHit hit;
        if (Physics.SphereCast(ray, _col.radius, out hit, 1))
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

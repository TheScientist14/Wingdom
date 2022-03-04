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
    [SerializeField] float flatTerrainTolerance = 0.2f;
    [SerializeField] float airTolerance = 0.01f;
    private Vector2 _movement = Vector2.zero;
    private bool _isGrounded = true;
    private bool _hasSpeedUp = false;

    //sound
    [System.Serializable]
    struct Footsteps
    {
        public AudioClip[] clips;
        public int layerInt;
    }

    [SerializeField] AudioSource source;
    [SerializeField] Footsteps[] footsteps;
    private int maxLayerInt;
    private Terrain _terrain;
    private int _xPosOnTerrainData;
    private int _zPosOnTerrainData;
    private float[] _terrainTextureValues;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _terrain = Terrain.activeTerrain;
        foreach (Footsteps footstep in footsteps)
        {
            if(footstep.layerInt > maxLayerInt)
            {
                maxLayerInt = footstep.layerInt;
            }
        }
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

    // source : https://johnleonardfrench.com/terrain-footsteps-in-unity-how-to-detect-different-textures/

    public void GetTerrainTexture()
    {
        ConvertPosition(transform.position);
        CheckTexture();
    }
    
    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - _terrain.transform.position;
        Vector3 mapPosition = new Vector3(terrainPosition.x / _terrain.terrainData.size.x, 0,
                                            terrainPosition.z / _terrain.terrainData.size.z);
        float xCoord = mapPosition.x * _terrain.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * _terrain.terrainData.alphamapHeight;
        _xPosOnTerrainData = (int)xCoord;
        _zPosOnTerrainData = (int)zCoord;
    }
    void CheckTexture()
    {
        float[,,] aMap = _terrain.terrainData.GetAlphamaps(_xPosOnTerrainData, _zPosOnTerrainData, 1, 1);
        for(int i = 0; i < maxLayerInt; i++)
        {
            _terrainTextureValues[i] = aMap[0, 0, i];
        }
    }

    public void PlayTerrainFootstep()
    {
        GetTerrainTexture();
        foreach(Footsteps footstep in footsteps)
        {
            if (_terrainTextureValues[footstep.layerInt] > 0)
            {
                source.PlayOneShot(GetRandomClip(footstep.clips), _terrainTextureValues[footstep.layerInt]);
            }
        }
    }

    AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}

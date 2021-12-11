using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BrokenTreeBehaviour : MonoBehaviour
{
    [SerializeField] MeshRenderer leaves;
    [SerializeField] Material healedMaterial;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI controlHint;

    private new Camera camera;
    private PlayerInput playerInput;

    private bool isHealed = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput.enabled = false;
        camera = Camera.main;
        canvas.transform.rotation = camera.transform.rotation;
        controlHint.gameObject.SetActive(false);
    }

    void OnInteract()
    {
        leaves.material = healedMaterial;
        isHealed = true;
        SetInteractable(false);
    }

    /*void OnCameraMove(){
        controlHint.transform.rotation = camera.transform.rotation;
        //controlHint.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y + 180, camera.transform.rotation.eulerAngles.z);
    }*/

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("tree");
        if (other.gameObject.CompareTag("Player"))
        {
            SetInteractable(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetInteractable(false);
        }
    }

    void SetInteractable(bool isInteractable)
    {
        if (!isHealed)
        {
            playerInput.enabled = isInteractable;
            controlHint.gameObject.SetActive(isInteractable);
        }
        else
        {
            playerInput.enabled = false;
            controlHint.gameObject.SetActive(false);
        }
    }
}

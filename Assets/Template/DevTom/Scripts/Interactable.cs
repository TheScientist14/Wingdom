using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerInput))]
public class Interactable : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI controlHint;

    private new Camera camera;
    private PlayerInput playerInput;
    private bool canBeInteractable = true;

    public UnityEvent hasBeenInteracted;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        hasBeenInteracted = new UnityEvent();
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
        if (canBeInteractable)
        {
            WaitForInteraction(false);
            hasBeenInteracted.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void OnCameraMove(){
        controlHint.transform.rotation = camera.transform.rotation;
        //controlHint.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, camera.transform.rotation.eulerAngles.y + 180, camera.transform.rotation.eulerAngles.z);
    }*/

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("help");
        if (other.gameObject.CompareTag("Player"))
        {
            WaitForInteraction(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WaitForInteraction(false);
        }
    }

    void WaitForInteraction(bool isInteractable)
    {
        if (canBeInteractable)
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

    public void SetIsInteractable(bool canBeInteractable)
    {
        this.canBeInteractable = canBeInteractable;
    }
}

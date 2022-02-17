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
    private bool canBeInteractable = true;
    private bool isInRange = false;
    private bool updateUiRotation = false;
    private ArrayList listeners;

    void Awake()
    {
        listeners = new ArrayList();
        camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (canvas && controlHint)
        {
            canvas.transform.rotation = camera.transform.rotation;
            controlHint.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No hint control set to " + name);
        }
    }

    void Update()
    {
        if (updateUiRotation)
        {
            if (canvas)
            {
                canvas.transform.rotation = camera.transform.rotation;
            }
        }
    }

    void OnInteract()
    {
        if (canBeInteractable && isInRange)
        {
            WaitForInteraction(false);
            Invoke();
        }
    }

    void OnCameraMovement(InputValue value){
        updateUiRotation = (value.Get<Vector2>().magnitude != 0);
    }

    void OnTriggerEnter(Collider other)
    {
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
            isInRange = isInteractable;
            if (controlHint)
            {
                controlHint.gameObject.SetActive(isInteractable);
            }
        }
        else
        {
            isInRange = false;
            if (controlHint)
            {
                controlHint.gameObject.SetActive(false);
            }
        }
    }

    public void SetIsInteractable(bool canBeInteractable)
    {
        this.canBeInteractable = canBeInteractable;
    }

    public void AddAction(UnityAction unityAction)
    {
        listeners.Add(unityAction);
    }

    public void RemoveAction(UnityAction unityAction)
    {
        listeners.Remove(unityAction);
    }

    private void Invoke()
    {
        foreach(UnityAction unityAction in listeners)
        {
            unityAction.Invoke();
        }
    }
}

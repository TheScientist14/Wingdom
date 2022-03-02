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
    [SerializeField] bool doSelfTrigger = false;

    private Camera _camera;
    private bool _canBeInteractable = true;
    private bool _isInRange = false;
    private bool _updateUiRotation = false;
    private ArrayList _listeners;

    void Awake()
    {
        _listeners = new ArrayList();
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (canvas && controlHint)
        {
            canvas.transform.rotation = _camera.transform.rotation;
            controlHint.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No hint control set to " + name);
        }
    }

    void Update()
    {
        if (_updateUiRotation)
        {
            if (canvas)
            {
                canvas.transform.rotation = _camera.transform.rotation;
            }
        }
    }

    void OnInteract()
    {
        if (_canBeInteractable && _isInRange)
        {
            WaitForInteraction(false);
            Invoke();
        }
    }

    void OnCameraMovement(InputValue value){
        _updateUiRotation = (value.Get<Vector2>().magnitude != 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (doSelfTrigger)
            {
                OnInteract();
            }
            else
            {
                WaitForInteraction(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!doSelfTrigger)
            {
                WaitForInteraction(false);
            }
        }
    }

    void WaitForInteraction(bool isInteractable)
    {
        if (_canBeInteractable)
        {
            _isInRange = isInteractable;
            if (controlHint)
            {
                controlHint.gameObject.SetActive(isInteractable);
            }
        }
        else
        {
            _isInRange = false;
            if (controlHint)
            {
                controlHint.gameObject.SetActive(false);
            }
        }
    }

    public void SetIsInteractable(bool canBeInteractable)
    {
        this._canBeInteractable = canBeInteractable;
    }

    public void AddAction(UnityAction unityAction)
    {
        _listeners.Add(unityAction);
    }

    public void RemoveAction(UnityAction unityAction)
    {
        _listeners.Remove(unityAction);
    }

    private void Invoke()
    {
        foreach(UnityAction unityAction in _listeners)
        {
            unityAction.Invoke();
        }
    }
}

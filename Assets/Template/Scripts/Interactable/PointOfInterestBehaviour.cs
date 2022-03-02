using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointOfInterestBehaviour : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [Serializable] struct TimedPos
    {
        public GameObject pos;
        public float time;
    }
    // first time won’t be considered
    [SerializeField] TimedPos[] travelPath;
    [SerializeField] new Camera camera;
    private Camera _mainCamera;

    private Vector3 _posBuffer;
    private Quaternion _rotationBuffer;
    private bool _isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        if(travelPath.Length >= 1)
        {
            travelPath[0].time = 0;
        }
        if (interactable != null)
        {
            interactable.AddAction(CameraCinematic);
        }
        else
        {
            Debug.LogWarning("No interactable set to " + gameObject.name);
        }
        _mainCamera = Camera.main;
    }

    public void CameraCinematic()
    {
        if (_isPlaying)
        {
            ResetCamera();
        }
        if(travelPath.Length >= 2)
        {
            GameManager.Instance.SetInputsActive(false);
            _mainCamera.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
            _posBuffer = camera.transform.position;
            _rotationBuffer = camera.transform.rotation;
            camera.transform.position = travelPath[0].pos.transform.position;
            camera.transform.rotation = travelPath[0].pos.transform.rotation;
            _isPlaying = true;
            StartCoroutine(MoveCamera());
        }
        else
        {
            Debug.Log("Can’t make camera travelling, less than 2 positions have been given in " + this.name);
        }
    }

    IEnumerator MoveCamera()
    {
        bool run = true;
        float timer = 0;
        int iPos = 0;
        TimedPos prev = travelPath[0];
        TimedPos next;
        next = travelPath[1];
        while (run)
        {
            camera.transform.position = Vector3.Lerp(prev.pos.transform.position, next.pos.transform.position, timer / next.time);
            camera.transform.rotation = Quaternion.Lerp(prev.pos.transform.rotation, next.pos.transform.rotation, timer / next.time);
            timer += Time.fixedDeltaTime;
            if(timer >= next.time)
            {
                prev = next;
                timer = 0;
                iPos++;
                if(iPos + 1 < travelPath.Length)
                {
                    next = travelPath[iPos + 1];
                }
                else
                {
                    run = false;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        ResetCamera();
    }

    public void ResetCamera()
    {
        StopCoroutine(MoveCamera());
        _isPlaying = false;
        camera.transform.position = _posBuffer;
        camera.transform.rotation = _rotationBuffer;
        camera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);
        GameManager.Instance.SetInputsActive(true);
    }

    public float GetLength()
    {
        float length = 0;
        foreach(TimedPos timedPos in travelPath)
        {
            length += timedPos.time;
        }
        return length;
    }
}

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
    private Camera mainCamera;

    private Vector3 posBuffer;
    private Quaternion rotationBuffer;
    //private float totalWeight = 0;

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
            Debug.LogError("No interactable set to " + gameObject.name);
        }
        mainCamera = Camera.main;
    }

    public void CameraCinematic()
    {
        StopCinematic();
        if(travelPath.Length >= 2)
        {
            GameManager.instance.SetInputsActive(false);
            mainCamera.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
            posBuffer = camera.transform.position;
            rotationBuffer = camera.transform.rotation;
            camera.transform.position = travelPath[0].pos.transform.position;
            camera.transform.rotation = travelPath[0].pos.transform.rotation;
            //Debug.Log("start");
            StartCoroutine(MoveCamera());
        }
        else
        {
            Debug.Log("Can’t make camera travelling, less than 2 positions have been given in " + this.name);
        }
    }

    public void StopCinematic()
    {
        StopCoroutine(MoveCamera());
        ResetCamera();
    }

    IEnumerator MoveCamera()
    {
        bool run = true;
        float timer = 0;
        int iPos = 0;
        float timeStep = 0.02f;
        TimedPos prev = travelPath[0];
        TimedPos next;
        next = travelPath[1];
        while (run)
        {
            camera.transform.position = Vector3.Lerp(prev.pos.transform.position, next.pos.transform.position, timer / next.time);
            camera.transform.rotation = Quaternion.Lerp(prev.pos.transform.rotation, next.pos.transform.rotation, timer / next.time);
            Debug.Log("top");
            timer += timeStep;
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
            yield return new WaitForSeconds(timeStep);
        }
        ResetCamera();
    }

    void ResetCamera()
    {
        camera.transform.position = posBuffer;
        camera.transform.rotation = rotationBuffer;
        camera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        GameManager.instance.SetInputsActive(true);
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

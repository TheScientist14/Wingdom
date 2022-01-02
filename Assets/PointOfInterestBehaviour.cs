using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointOfInterestBehaviour : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] GameObject initPos;
    [SerializeField] GameObject endPos;
    [SerializeField] float sequenceDuration = 5;
    [SerializeField] LockCamera lockCamera;
    private new Camera camera;

    private Vector3 posBuffer;
    private Quaternion rotationBuffer;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (interactable != null)
            interactable.AddAction(CameraCinematic);
        camera = Camera.main;
    }

    // Update is called once per frame
    void CameraCinematic()
    {
        GameManager.instance.SetInputsActive(false);
        posBuffer = camera.transform.position;
        rotationBuffer = camera.transform.rotation;
        lockCamera.enabled = false;
        camera.transform.position = initPos.transform.position;
        camera.transform.rotation = initPos.transform.rotation;
        camera.orthographic = false;
        Debug.Log("start");
        timer = 0;
        StartCoroutine(MoveCamera());
        camera.transform.DORotate(endPos.transform.rotation.eulerAngles, sequenceDuration).OnComplete(ResetCamera);
    }

    void ResetCamera()
    {
        Debug.Log("stop");
        camera.transform.position = posBuffer;
        camera.transform.rotation = rotationBuffer;
        camera.orthographic = true;
        lockCamera.enabled = true;
        GameManager.instance.SetInputsActive(true);
    }

    IEnumerator MoveCamera()
    {
        bool run = true;
        while (run)
        {
            camera.transform.position += (endPos.transform.position - initPos.transform.position) / sequenceDuration * 0.05f;
            Debug.Log("top");
            timer += 0.05f;
            if(timer > sequenceDuration)
            {
                run = false;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}

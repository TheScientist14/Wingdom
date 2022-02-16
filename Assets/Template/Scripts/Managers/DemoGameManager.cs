using UnityEngine;
using UnityEngine.InputSystem;

public class DemoGameManager : GameManager
{
    private float noInputTimeBeforeSleep = 10;
    private float noInputCooldown = 0;
    private Vector3 lastMousePos = Vector3.zero;
    private bool hasEverMoved = false;
    private bool isPlayingCinematic = false;
    private float cinematicDuration;

    void Update()
    {
        if (!isPlayingCinematic)
        {
            if (Input.anyKey || lastMousePos != Input.mousePosition)
            {
                noInputCooldown = 0;
                //hasEverMoved = true;
            }
            else
            {
                Debug.Log("sleeping");
                noInputCooldown += Time.deltaTime;
            }
            if(noInputCooldown >= noInputTimeBeforeSleep)
            {
                if (hasEverMoved)
                {
                    Restart();
                    noInputCooldown = 0;
                    hasEverMoved = false;
                }
                else
                {
                    StartCinematic();
                }
            }
        }
        else
        {
            if (Input.anyKey)
            {
                noInputCooldown = 0;
                StopCinematic();
            }
            else
            {
                Debug.Log("cinematic");
                noInputCooldown += Time.deltaTime;
                if(noInputCooldown >= cinematicDuration)
                {
                    StopCinematic();
                    StartCinematic();
                }
            }
        }
        lastMousePos = Input.mousePosition;
    }

    void OnReload()
    {
        Restart();
    }

    void StartCinematic()
    {
        noInputCooldown = 0;
        isPlayingCinematic = true;
        GameObject poi = GameObject.FindGameObjectWithTag("Cinematic");
        if (poi && poi.GetComponent<PointOfInterestBehaviour>())
        {
            PointOfInterestBehaviour poiBehaviour = poi.GetComponent<PointOfInterestBehaviour>();
            poiBehaviour.CameraCinematic();
            cinematicDuration = poiBehaviour.GetLength();
        }
    }

    void StopCinematic()
    {
        isPlayingCinematic = false;
        GameObject poi = GameObject.FindGameObjectWithTag("Cinematic");
        if (poi && poi.GetComponent<PointOfInterestBehaviour>())
        {
            poi.GetComponent<PointOfInterestBehaviour>().StopCinematic();
        }
    }
}

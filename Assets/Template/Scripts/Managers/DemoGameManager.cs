using UnityEngine;
using UnityEngine.InputSystem;

public class DemoGameManager : GameManager
{
    [SerializeField] BubbleSpeech storyIntroductionSpeech;

    private float noInputTimeBeforeSleep = 60;
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
                if (!hasEverMoved)
                {
                    hasEverMoved = true;
                    DialogManager.instance.StartDialog(storyIntroductionSpeech);
                }
            }
            else
            {
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
                DialogManager.instance.StartDialog(storyIntroductionSpeech);
            }
            else
            {
                noInputCooldown += Time.deltaTime;
                if(noInputCooldown >= cinematicDuration + 0.1f)
                {
                    StartCinematic();
                }
            }
        }
        lastMousePos = Input.mousePosition;
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
            poi.GetComponent<PointOfInterestBehaviour>().ResetCamera();
        }
    }
}

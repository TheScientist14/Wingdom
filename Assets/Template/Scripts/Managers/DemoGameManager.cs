using UnityEngine;
using UnityEngine.InputSystem;

public class DemoGameManager : GameManager
{
    [SerializeField] BubbleSpeech storyIntroductionSpeech;

    private float _noInputTimeBeforeSleep = 60;
    private float _noInputCooldown = 0;
    private Vector3 _lastMousePos = Vector3.zero;
    private bool _hasEverMoved = false;
    private bool _isPlayingCinematic = false;
    private float _cinematicDuration;
    private float _restartBuffer = 0;

    void Update()
    {
        if (!_isPlayingCinematic)
        {
            if (Input.anyKey || _lastMousePos != Input.mousePosition)
            {
                _noInputCooldown = 0;
                if (!_hasEverMoved)
                {
                    _hasEverMoved = true;
                    DialogManager.Instance.StartDialog(storyIntroductionSpeech);
                }
            }
            else
            {
                _noInputCooldown += Time.deltaTime;
            }
            if(_noInputCooldown >= _noInputTimeBeforeSleep)
            {
                if (_hasEverMoved)
                {
                    Restart();
                    _noInputCooldown = 0;
                    _hasEverMoved = false;
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
                _noInputCooldown = 0;
                StopCinematic();
                DialogManager.Instance.StartDialog(storyIntroductionSpeech);
            }
            else
            {
                _noInputCooldown += Time.deltaTime;
                if(_noInputCooldown >= _cinematicDuration + 0.1f)
                {
                    StartCinematic();
                }
            }
        }
        if (Input.GetKey(KeyCode.Delete))
        {
            _restartBuffer += Time.deltaTime;
            if(_restartBuffer > 1)
            {
                _restartBuffer = 0;
                Restart();
            }
        }
        else
        {
            _restartBuffer = 0;
        }
        _lastMousePos = Input.mousePosition;
    }

    void StartCinematic()
    {
        _noInputCooldown = 0;
        _isPlayingCinematic = true;
        GameObject poi = GameObject.FindGameObjectWithTag("Cinematic");
        if (poi && poi.GetComponent<PointOfInterestBehaviour>())
        {
            PointOfInterestBehaviour poiBehaviour = poi.GetComponent<PointOfInterestBehaviour>();
            poiBehaviour.CameraCinematic();
            _cinematicDuration = poiBehaviour.GetLength();
        }
    }

    void StopCinematic()
    {
        _isPlayingCinematic = false;
        GameObject poi = GameObject.FindGameObjectWithTag("Cinematic");
        if (poi && poi.GetComponent<PointOfInterestBehaviour>())
        {
            poi.GetComponent<PointOfInterestBehaviour>().ResetCamera();
        }
    }
}

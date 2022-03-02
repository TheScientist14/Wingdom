using System;
using UnityEngine;

public class TriggerFireBehaviour : MonoBehaviour
{
    [SerializeField] private PointOfInterestBehaviour fireCinematic;
    [SerializeField] private Collider trigger;
    [SerializeField] private BubbleSpeech[] enableFireTriggerSpeeches;

    private FireBehaviour[] _fires;

    // Start is called before the first frame update
    private void Start()
    {
        _fires = FindObjectsOfType<FireBehaviour>();
        foreach (FireBehaviour fire in _fires)
        {
            fire.gameObject.SetActive(false);
        }
        trigger.enabled = false;
        DialogManager.Instance.onBubbleShown.AddListener(EnableTriggerOnSpeech);
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (FireBehaviour fire in _fires)
        {
            fire.gameObject.SetActive(true);
        }
        fireCinematic.CameraCinematic();
        gameObject.SetActive(false);
    }

    private void EnableTriggerOnSpeech(BubbleSpeech shownSpeech)
    {
        if (Array.IndexOf(enableFireTriggerSpeeches, shownSpeech) != -1)
        {
            trigger.enabled = true;
            DialogManager.Instance.onBubbleShown.RemoveListener(EnableTriggerOnSpeech);
        }
    }
}

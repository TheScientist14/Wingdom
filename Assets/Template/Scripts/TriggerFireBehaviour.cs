using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFireBehaviour : MonoBehaviour
{
    [SerializeField] PointOfInterestBehaviour fireCinematic;
    [SerializeField] Collider trigger;
    [SerializeField] BubbleSpeech[] enableFireTriggerSpeeches;

    private FireBehaviour[] fires;

    // Start is called before the first frame update
    void Start()
    {
        fires = FindObjectsOfType<FireBehaviour>();
        foreach (FireBehaviour fire in fires)
        {
            fire.gameObject.SetActive(false);
        }
        trigger.enabled = false;
        DialogManager.instance.onBubbleShown.AddListener(EnableTriggerOnSpeech);
    }

    void OnTriggerEnter()
    {
        foreach (FireBehaviour fire in fires)
        {
            fire.gameObject.SetActive(true);
        }
        fireCinematic.CameraCinematic();
        gameObject.SetActive(false);
    }

    void EnableTriggerOnSpeech(BubbleSpeech shownSpeech)
    {
        if (Array.IndexOf(enableFireTriggerSpeeches, shownSpeech) != -1)
        {
            trigger.enabled = true;
            DialogManager.instance.onBubbleShown.RemoveListener(EnableTriggerOnSpeech);
        }
    }
}

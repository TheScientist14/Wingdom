using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBehaviour : MonoBehaviour
{
    [SerializeField] GameObject fire;
    [SerializeField] Interactable interactable;
    [SerializeField] BubbleSpeech initFireSpeech;

    public UnityEvent onFireKilled;

    // Start is called before the first frame update
    void Start()
    {
        fire.SetActive(false);
        interactable.AddAction(KillFire);
        DialogManager.instance.onBubbleShown.AddListener(InitFireOnDialog);
    }

    void KillFire()
    {
        fire.SetActive(false);
        onFireKilled.Invoke();
    }

    public bool IsAlive()
    {
        return fire.activeSelf;
    }

    void InitFireOnDialog(BubbleSpeech shownSpeech)
    {
        if (shownSpeech.Equals(initFireSpeech))
        {
            fire.SetActive(true);
            DialogManager.instance.onBubbleShown.RemoveListener(InitFireOnDialog);
        }
    }
}

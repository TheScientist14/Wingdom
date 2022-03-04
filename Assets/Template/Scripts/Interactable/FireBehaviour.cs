using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBehaviour : MonoBehaviour
{
    [SerializeField] GameObject fire;
    [SerializeField] AudioSource speaker;
    public Interactable interactable;

    public UnityEvent onFireKilled;

    // Start is called before the first frame update
    void Start()
    {
        interactable.SetIsInteractable(false);
        interactable.AddAction(KillFire);
    }

    void KillFire()
    {
        fire.SetActive(false);
        interactable.SetIsInteractable(false);
        onFireKilled.Invoke();
        speaker.Stop();
    }

    public bool IsAlive()
    {
        return fire.activeInHierarchy;
    }
}

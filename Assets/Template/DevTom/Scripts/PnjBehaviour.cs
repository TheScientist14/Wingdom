using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjBehaviour : MonoBehaviour
{
    [SerializeField] Monolog dialog;
    [SerializeField] Interactable interactable;

    // Start is called before the first frame update
    protected void Start()
    {
        interactable.AddAction(Talk);
    }

    protected virtual void Talk()
    {
        Debug.Log("Pnj");
        DialogManager.instance.StartDialog(dialog);
    }
}

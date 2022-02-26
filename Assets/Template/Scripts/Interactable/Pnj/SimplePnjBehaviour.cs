using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePnjBehaviour : MonoBehaviour
{
    [SerializeField] Monolog dialog;
    [SerializeField] Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        if (interactable != null)
        {
            interactable.AddAction(Talk);
        }
    }

    void Talk()
    {
        DialogManager.instance.StartDialog(dialog);
    }
}

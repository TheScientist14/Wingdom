using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePnjBehaviour : MonoBehaviour
{
    [SerializeField] Monolog dialog;
    [SerializeField] Interactable interactable;

    // Start is called before the first frame update
    protected void Start()
    {
        if (interactable != null)
        {
            interactable.AddAction(Talk);
        }
    }

    protected void Talk()
    {
        Debug.Log("Pnj");
        DialogManager.instance.StartDialog(dialog);
        /*if (interactionClip)
        {
            SoundManager.instance.PlayClip(interactionClip);
        }*/
    }
}

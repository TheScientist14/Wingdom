using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class BrokenTreeBehaviour : MonoBehaviour
{
    [SerializeField] MeshRenderer leaves;
    [SerializeField] Material healedMaterial;
    [SerializeField] Interactable interactionListener;

    private bool _isHealed = false;

    public UnityEvent onHeal;

    // Start is called before the first frame update
    void Start()
    {
        interactionListener.AddAction(Heal);
        leaves.gameObject.SetActive(false);
        interactionListener.SetIsInteractable(false);
    }

    void Heal()
    {
        //leaves.material = healedMaterial;
        leaves.gameObject.SetActive(true);
        _isHealed = true;
        interactionListener.SetIsInteractable(false);
        onHeal.Invoke();
    }

    public bool HasBeenHealed()
    {
        return _isHealed;
    }

    public void SetCanBeHealed(bool canBeHealed)
    {
        interactionListener.SetIsInteractable(canBeHealed);
    }
}

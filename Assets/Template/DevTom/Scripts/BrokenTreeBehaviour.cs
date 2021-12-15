using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BrokenTreeBehaviour : MonoBehaviour
{
    [SerializeField] MeshRenderer leaves;
    [SerializeField] Material healedMaterial;
    [SerializeField] Interactable interactionListener;

    //private bool isHealed = false;

    // Start is called before the first frame update
    void Start()
    {
        interactionListener.AddAction(Heal);
        leaves.gameObject.SetActive(false);
    }

    void Heal()
    {
        //leaves.material = healedMaterial;
        leaves.gameObject.SetActive(true);
        //isHealed = true;
        interactionListener.SetIsInteractable(false);
    }
}

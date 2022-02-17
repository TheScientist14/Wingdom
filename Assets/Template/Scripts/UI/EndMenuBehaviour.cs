using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndMenuBehaviour : MonoBehaviour
{
    [SerializeField] GameObject endMenuPanel;
    [SerializeField] GameObject firstSelected;
    [SerializeField] BubbleSpeech[] requiredSpeeches;
    private HashSet<BubbleSpeech> remainingRequiredSpeeches;

    // Start is called before the first frame update
    void Start()
    {
        remainingRequiredSpeeches = new HashSet<BubbleSpeech>(requiredSpeeches);
        /*foreach(BubbleSpeech speech in requiredSpeeches)
        {
            remainingRequiredSpeeches.Add(speech);
        }*/
        DialogManager.instance.onBubbleShown.AddListener(CheckForEnd);
        endMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void CheckForEnd(BubbleSpeech shownSpeech)
    {
        remainingRequiredSpeeches.Remove(shownSpeech);
        if(remainingRequiredSpeeches.Count == 0)
        {
            Debug.Log("won");
            GameManager.instance.SetInputsActive(false);
            endMenuPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    public void Restart()
    {
        GameManager.instance.Restart();
    }
}

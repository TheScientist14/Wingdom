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
    private HashSet<BubbleSpeech> _remainingRequiredSpeeches;

    // Start is called before the first frame update
    void Start()
    {
        _remainingRequiredSpeeches = new HashSet<BubbleSpeech>(requiredSpeeches);
        /*foreach(BubbleSpeech speech in requiredSpeeches)
        {
            remainingRequiredSpeeches.Add(speech);
        }*/
        DialogManager.Instance.onBubbleShown.AddListener(CheckForEnd);
        endMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void CheckForEnd(BubbleSpeech shownSpeech)
    {
        _remainingRequiredSpeeches.Remove(shownSpeech);
        if(_remainingRequiredSpeeches.Count == 0)
        {
            Debug.Log("won");
            GameManager.Instance.SetInputsActive(false);
            endMenuPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    public void Restart()
    {
        GameManager.Instance.Restart();
    }
}

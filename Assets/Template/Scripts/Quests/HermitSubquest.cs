using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitSubquest : Quest
{
    [SerializeField] BubbleSpeech startQuestSpeech;
    [SerializeField] GameObject applesBasket;

    Interactable applesBasketInteractable;

    void Awake()
    {
        applesBasketInteractable = applesBasket.GetComponent<Interactable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (applesBasketInteractable)
        {
            applesBasketInteractable.AddAction(CompleteQuest);
        }
        applesBasket.SetActive(false);
        DialogManager.instance.onBubbleShown.AddListener(StartSubquestOnSpeech);
    }

    void StartSubquestOnSpeech(BubbleSpeech shownSpeech)
    {
        if (shownSpeech.Equals(startQuestSpeech))
        {
            SetProgress(QuestState.Started);
            applesBasket.SetActive(true);
        }
    }

    void CompleteQuest()
    {
        SetProgress(QuestState.Completed);
        applesBasket.SetActive(false);
    }
}

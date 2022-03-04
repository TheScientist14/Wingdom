using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitSubquest : Quest
{
    [SerializeField] BubbleSpeech startQuestSpeech;
    [SerializeField] GameObject applesBasket;

    Interactable _applesBasketInteractable;

    void Awake()
    {
        _applesBasketInteractable = applesBasket.GetComponent<Interactable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_applesBasketInteractable)
        {
            _applesBasketInteractable.AddAction(CompleteQuest);
        }
        applesBasket.SetActive(false);
        DialogManager.Instance.onBubbleShown.AddListener(StartSubquestOnSpeech);
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

    public override string GetQuestName()
    {
        return "Where are my apple";
    }
    
    public override string GetQuestDetail()
    {
        return "David the ermit agree to come back to the village only if you bring to him some apple. You will be able to find those near the path." +
               " He is hungry, hurry up !";
    }
}

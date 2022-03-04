using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimmyQuest : Quest
{
    [SerializeField] BubbleSpeech acceptQuestSpeech;
    [SerializeField] Interactable targetInteractable;
    [SerializeField] Interactable[] interactableActiveOnlyWhileQuest;

    // Start is called before the first frame update
    void Start()
    {
        DialogManager.Instance.onBubbleShown.AddListener(StartQuestOnSpeech);
        targetInteractable.SetIsInteractable(false);
        foreach(Interactable interactable in interactableActiveOnlyWhileQuest)
        {
            interactable.SetIsInteractable(false);
        }
    }

    void StartQuestOnSpeech(BubbleSpeech shownBubble)
    {
        if (shownBubble.Equals(acceptQuestSpeech))
        {
            SetProgress(QuestState.Started);
            DialogManager.Instance.onBubbleShown.RemoveListener(StartQuestOnSpeech);
            targetInteractable.AddAction(EndQuest);
            targetInteractable.SetIsInteractable(true);
            foreach (Interactable interactable in interactableActiveOnlyWhileQuest)
            {
                interactable.SetIsInteractable(true);
            }
        }
    }

    void EndQuest()
    {
        SetProgress(QuestState.Completed);
        targetInteractable.SetIsInteractable(false);
        foreach (Interactable interactable in interactableActiveOnlyWhileQuest)
        {
            interactable.SetIsInteractable(false);
        }
    }

    public override string GetQuestName()
    {
        return "Timmy is missing !";
    }
    
    public override string GetQuestDetail()
    {
        return "A child called Timmy lost himself during his search for wood. The mayor Samug asked you to find him and bring him back to the village." +
               " You should talk to the villager to see if somebody saw him";
    }
}

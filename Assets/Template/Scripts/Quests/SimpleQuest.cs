using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleQuest : Quest
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
}

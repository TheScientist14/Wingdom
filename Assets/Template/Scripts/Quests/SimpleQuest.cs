using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleQuest : Quest
{
    [SerializeField] BubbleSpeech acceptQuestSpeech;
    [SerializeField] Interactable targetInteractable;

    // Start is called before the first frame update
    void Start()
    {
        DialogManager.instance.onBubbleShown.AddListener(StartQuestOnSpeech);
    }

    void StartQuestOnSpeech(BubbleSpeech shownBubble)
    {
        if (shownBubble.Equals(acceptQuestSpeech))
        {
            SetProgress(QuestState.Started);
            DialogManager.instance.onBubbleShown.RemoveListener(StartQuestOnSpeech);
            targetInteractable.AddAction(EndQuest);
        }
    }

    void EndQuest()
    {
        SetProgress(QuestState.Completed);
    }
}

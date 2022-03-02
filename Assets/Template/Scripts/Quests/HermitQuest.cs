using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitQuest : Quest
{
    [SerializeField] BubbleSpeech acceptQuestSpeech;
    [SerializeField] BubbleSpeech success1QuestSpeech;
    [SerializeField] BubbleSpeech success2QuestSpeech;
    [SerializeField] BubbleSpeech failQuestSpeech;
    [SerializeField] HermitPnjBehaviour hermitBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        DialogManager.Instance.onBubbleShown.AddListener(StartQuestOnSpeech);
    }

    void StartQuestOnSpeech(BubbleSpeech shownSpeech)
    {
        if (shownSpeech.Equals(acceptQuestSpeech))
        {
            SetProgress(QuestState.Started);
        }
        if(shownSpeech.Equals(success1QuestSpeech) || shownSpeech.Equals(success2QuestSpeech))
        {
            Debug.Log("success hermit");
            SetProgress(QuestState.Completed);
            hermitBehaviour.OnQuestSuccess();
        }
        if (shownSpeech.Equals(failQuestSpeech))
        {
            SetProgress(QuestState.Failed);
        }
    }
}

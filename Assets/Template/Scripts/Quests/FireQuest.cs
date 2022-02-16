using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireQuest : Quest
{
    [SerializeField] BubbleSpeech startSpeech;

    private FireBehaviour[] fires;

    // Start is called before the first frame update
    void Start()
    {
        DialogManager.instance.onBubbleShown.AddListener(StartQuestOnDialog);
        fires = FindObjectsOfType<FireBehaviour>();
    }

    // Update is called once per frame
    void UpdateQuestState()
    {
        bool firesHaveBeenKilled = true;
        foreach (FireBehaviour fire in fires)
        {
            if (fire.IsAlive())
            {
                firesHaveBeenKilled = false;
                break;
            }
        }
        if (firesHaveBeenKilled)
        {
            SetProgress(QuestState.Completed);
        }
    }

    void StartQuestOnDialog(BubbleSpeech shownSpeech)
    {
        if (shownSpeech.Equals(startSpeech))
        {
            SetProgress(QuestState.Started);
            DialogManager.instance.onBubbleShown.RemoveListener(StartQuestOnDialog);
            foreach (FireBehaviour fire in fires)
            {
                fire.onFireKilled.AddListener(UpdateQuestState);
            }
        }
    }
}

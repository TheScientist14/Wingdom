using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireQuest : Quest
{
    [SerializeField] BubbleSpeech startSpeech;

    private FireBehaviour[] fires;

    void Awake()
    {
        fires = FindObjectsOfType<FireBehaviour>();
        Debug.Log(fires.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogManager.instance.onBubbleShown.AddListener(StartQuestOnDialog);
    }

    // Update is called once per frame
    void UpdateQuestState()
    {
        bool firesHaveBeenKilled = true;
        foreach (FireBehaviour fire in fires)
        {
            Debug.Log(fire.gameObject.name);
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
            Debug.Log("Starting quest");
            foreach (FireBehaviour fire in fires)
            {
                fire.onFireKilled.AddListener(UpdateQuestState);
                fire.interactable.SetIsInteractable(true);
                Debug.Log("Listening " + fire.gameObject.name);
            }
            DialogManager.instance.onBubbleShown.RemoveListener(StartQuestOnDialog);
        }
    }
}

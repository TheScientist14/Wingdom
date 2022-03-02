using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireQuest : Quest
{
    [SerializeField] BubbleSpeech startSpeech;

    private FireBehaviour[] _fires;

    void Awake()
    {
        _fires = FindObjectsOfType<FireBehaviour>();
        Debug.Log(_fires.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogManager.Instance.onBubbleShown.AddListener(StartQuestOnDialog);
    }

    // Update is called once per frame
    void UpdateQuestState()
    {
        bool firesHaveBeenKilled = true;
        foreach (FireBehaviour fire in _fires)
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
            foreach (FireBehaviour fire in _fires)
            {
                fire.onFireKilled.AddListener(UpdateQuestState);
                fire.interactable.SetIsInteractable(true);
                Debug.Log("Listening " + fire.gameObject.name);
            }
            DialogManager.Instance.onBubbleShown.RemoveListener(StartQuestOnDialog);
        }
    }
}

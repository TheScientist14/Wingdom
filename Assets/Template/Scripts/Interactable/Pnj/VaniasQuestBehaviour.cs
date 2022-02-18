using UnityEngine;

public class VaniasQuestBehaviour : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] Quest quest;
    [SerializeField] BubbleSpeech startingQuestDialog;
    [SerializeField] BubbleSpeech propositionQuestDialog;
    [SerializeField] BubbleSpeech onGoingQuestDialog;
    [SerializeField] BubbleSpeech completedQuestDialog;
    [SerializeField] BubbleSpeech completedQuestReminderDialog;

    bool endingQuestDialogHasBeenShown = false;
    bool hasTalkOnce = false;

    void Start()
    {
        interactable.AddAction(Talk);
        interactable.SetIsInteractable(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTalkOnce)
        {
            Talk();
            interactable.SetIsInteractable(true);
            hasTalkOnce = true;
        }
    }

    protected void Talk()
    {
        switch (quest.getProgress())
        {
            case Quest.QuestState.Unknown:
                DialogManager.instance.StartDialog(startingQuestDialog);
                quest.SetProgress(Quest.QuestState.Unaccepted);
                break;
            case Quest.QuestState.Unaccepted:
                DialogManager.instance.StartDialog(propositionQuestDialog);
                break;
            case Quest.QuestState.Started:
                DialogManager.instance.StartDialog(onGoingQuestDialog);
                break;
            case Quest.QuestState.Completed:
                if (!endingQuestDialogHasBeenShown)
                {
                    DialogManager.instance.StartDialog(completedQuestDialog);
                    endingQuestDialogHasBeenShown = true;
                }
                else
                {
                    DialogManager.instance.StartDialog(completedQuestReminderDialog);
                }
                break;
        }
    }
}

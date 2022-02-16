using UnityEngine;

public class VaniasQuestBehaviour : MonoBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] BubbleSpeech startingQuestDialog;
    [SerializeField] BubbleSpeech propositionQuestDialog;
    [SerializeField] BubbleSpeech onGoingQuestDialog;
    [SerializeField] BubbleSpeech failedQuestDialog;
    [SerializeField] BubbleSpeech failedQuestReminderDialog;
    [SerializeField] BubbleSpeech completedQuestDialog;
    [SerializeField] BubbleSpeech completedQuestReminderDialog;

    bool endingQuestDialogHasBeenShown = false;

    private void OnTriggerEnter(Collider other)
    {
        Talk();
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
            case Quest.QuestState.Failed:
                if (!endingQuestDialogHasBeenShown)
                {
                    DialogManager.instance.StartDialog(failedQuestDialog);
                    endingQuestDialogHasBeenShown = true;
                }
                else
                {
                    DialogManager.instance.StartDialog(failedQuestReminderDialog);
                }
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

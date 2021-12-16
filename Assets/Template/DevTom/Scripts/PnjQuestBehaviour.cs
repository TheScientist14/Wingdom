using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjQuestBehaviour : PnjBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] Monolog startingQuestDialog;
    [SerializeField] Monolog onGoingQuestDialog;
    [SerializeField] Monolog failedQuestDialog;
    [SerializeField] Monolog failedQuestReminderDialog;
    [SerializeField] Monolog completedQuestDialog;
    [SerializeField] Monolog completedQuestReminderDialog;

    bool endingQuestDialogHasBeenShown = false;

   protected override void Talk()
    {
        switch (quest.getProgress())
        {
            case Quest.QuestState.NotStarted:
                DialogManager.instance.StartDialog(startingQuestDialog);
                quest.StartQuest();
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
            case Quest.QuestState.Uncompleted:

                break;
        }
    }
}

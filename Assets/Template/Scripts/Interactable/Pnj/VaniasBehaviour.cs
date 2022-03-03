using UnityEngine;

public class VaniasBehaviour : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] Quest quest;
    [SerializeField] BubbleSpeech startingQuestDialog;
    [SerializeField] BubbleSpeech propositionQuestDialog;
    [SerializeField] BubbleSpeech onGoingQuestDialog;
    [SerializeField] BubbleSpeech completedQuestDialog;
    [SerializeField] BubbleSpeech completedQuestReminderDialog;

    bool _endingQuestDialogHasBeenShown = false;
    bool _hasTalkOnce = false;

    void Start()
    {
        interactable.AddAction(Talk);
        interactable.SetIsInteractable(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasTalkOnce)
        {
            Talk();
            interactable.SetIsInteractable(true);
            _hasTalkOnce = true;
        }
    }

    protected void Talk()
    {
        switch (quest.GetProgress())
        {
            case Quest.QuestState.Unknown:
                DialogManager.Instance.StartDialog(startingQuestDialog);
                quest.SetProgress(Quest.QuestState.Unaccepted);
                break;
            case Quest.QuestState.Unaccepted:
                DialogManager.Instance.StartDialog(propositionQuestDialog);
                break;
            case Quest.QuestState.Started:
                DialogManager.Instance.StartDialog(onGoingQuestDialog);
                break;
            case Quest.QuestState.Completed:
                if (!_endingQuestDialogHasBeenShown)
                {
                    DialogManager.Instance.StartDialog(completedQuestDialog);
                    _endingQuestDialogHasBeenShown = true;
                }
                else
                {
                    DialogManager.Instance.StartDialog(completedQuestReminderDialog);
                }
                break;
        }
    }
}

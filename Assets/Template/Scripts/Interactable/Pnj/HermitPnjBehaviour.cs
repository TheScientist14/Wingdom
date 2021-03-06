using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitPnjBehaviour : MonoBehaviour
{
    [SerializeField] Interactable interactable;
    [SerializeField] BubbleSpeech beforeQuestSpeech;
    [SerializeField] BubbleSpeech firstSpeech;
    [SerializeField] BubbleSpeech subquestInProgressSpeech;
    [SerializeField] BubbleSpeech subquestDoneSpeech;
    [SerializeField] BubbleSpeech failedQuestSpeech;
    [SerializeField] Transform successTransform;
    [SerializeField] Quest hermitQuest;
    [SerializeField] Quest hermitSubquest;

    // Start is called before the first frame update
    void Start()
    {
        interactable.AddAction(Talk);
    }

    void Talk()
    {
        switch (hermitQuest.GetProgress())
        {
            case Quest.QuestState.Unknown:
            case Quest.QuestState.Unaccepted:
                DialogManager.Instance.StartDialog(beforeQuestSpeech);
                break;
            case Quest.QuestState.Started:
                switch (hermitSubquest.GetProgress())
                {
                    case Quest.QuestState.Unknown:
                    case Quest.QuestState.Unaccepted:
                        DialogManager.Instance.StartDialog(firstSpeech);
                        break;
                    case Quest.QuestState.Started:
                        DialogManager.Instance.StartDialog(subquestInProgressSpeech);
                        break;
                    case Quest.QuestState.Completed:
                        DialogManager.Instance.StartDialog(subquestDoneSpeech);
                        break;
                }
                break;
            case Quest.QuestState.Failed:
                DialogManager.Instance.StartDialog(failedQuestSpeech);
                break;
            default:
                break;
        }
    }

    public void OnQuestSuccess()
    {
        interactable.SetIsInteractable(false);
        Invoke(nameof(Teleport), 5);
    }

    void Teleport()
    {
        transform.position = successTransform.position;
        transform.rotation = successTransform.rotation;
    }
}

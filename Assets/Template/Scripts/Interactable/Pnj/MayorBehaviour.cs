using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayorBehaviour : MonoBehaviour
{
    [SerializeField] Interactable interactable;

    [Serializable]
    public struct QuestBubble
    {
        public Quest quest;
        public BubbleSpeech startingSpeech;
        public BubbleSpeech inProgressSpeech;
        public BubbleSpeech failedSpeech;
        public BubbleSpeech completedSpeech;
    }

    [SerializeField] QuestBubble[] questsBubbles;
    [SerializeField] BubbleSpeech allQuestsCompletedSpeech;

    private int _iQuest = 0;

    // Start is called before the first frame update
    void Start()
    {
        interactable.AddAction(Talk);
    }

    void Talk()
    {
        Debug.Log(_iQuest);
        if(_iQuest < questsBubbles.Length)
        {
            Debug.Log(questsBubbles[_iQuest].quest.GetProgress());
            switch (questsBubbles[_iQuest].quest.GetProgress())
            {
                case Quest.QuestState.Unknown:
                case Quest.QuestState.Unaccepted:
                    DialogManager.Instance.StartDialog(questsBubbles[_iQuest].startingSpeech);
                    break;
                case Quest.QuestState.Started:
                    DialogManager.Instance.StartDialog(questsBubbles[_iQuest].inProgressSpeech);
                    break;
                case Quest.QuestState.Failed:
                    if (questsBubbles[_iQuest].failedSpeech)
                    {
                        DialogManager.Instance.StartDialog(questsBubbles[_iQuest].failedSpeech);
                        _iQuest++;
                    }
                    else
                    {
                        StartCompletedSpeechCurrentQuest();
                    }
                    break;
                case Quest.QuestState.Completed:
                    StartCompletedSpeechCurrentQuest();
                    break;
            }
        }
        else
        {
            DialogManager.Instance.StartDialog(allQuestsCompletedSpeech);
        }
    }

    void StartCompletedSpeechCurrentQuest()
    {
        if (questsBubbles[_iQuest].completedSpeech)
        {
            DialogManager.Instance.StartDialog(questsBubbles[_iQuest].completedSpeech);
            _iQuest++;
        }
        else
        {
            _iQuest++;
            Talk();
        }
    }
}

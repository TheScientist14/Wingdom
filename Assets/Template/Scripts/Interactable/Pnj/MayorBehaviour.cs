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
        public BubbleSpeech completedSpeech;
    }

    [SerializeField] QuestBubble[] questsBubbles;
    [SerializeField] BubbleSpeech allQuestsCompletedSpeech;

    private int iQuest = 0;

    // Start is called before the first frame update
    void Start()
    {
        interactable.AddAction(Talk);
    }

    void Talk()
    {
        Debug.Log(iQuest);
        if(iQuest < questsBubbles.Length)
        {
            Debug.Log(questsBubbles[iQuest].quest.getProgress());
            switch (questsBubbles[iQuest].quest.getProgress())
            {
                case Quest.QuestState.Unknown:
                case Quest.QuestState.Unaccepted:
                    DialogManager.instance.StartDialog(questsBubbles[iQuest].startingSpeech);
                    break;
                case Quest.QuestState.Started:
                    DialogManager.instance.StartDialog(questsBubbles[iQuest].inProgressSpeech);
                    break;
                case Quest.QuestState.Completed:
                    DialogManager.instance.StartDialog(questsBubbles[iQuest].completedSpeech);
                    iQuest++;
                    break;
            }
        }
        else
        {
            DialogManager.instance.StartDialog(allQuestsCompletedSpeech);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    protected QuestState state = QuestState.NotStarted;

    public enum QuestState
    {
        NotStarted, Started, Failed, Completed
    }

    public QuestState getProgress()
    {
        return state;
    }

    public virtual void StartQuest()
    {
        state = QuestState.Started;
    }

    public abstract void EndQuest();
}

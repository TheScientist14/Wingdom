using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    protected QuestState state = QuestState.Unknown;

    public enum QuestState
    {
        Unknown, Unaccepted, Started, Failed, Completed
    }

    public QuestState getProgress()
    {
        return state;
    }

    public void SetProgress(QuestState newState)
    {
        state = newState;
    }

    public virtual void StartQuest()
    {
        state = QuestState.Started;
    }

    public abstract void EndQuest();
}

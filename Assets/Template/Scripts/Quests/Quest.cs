using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : MonoBehaviour
{
    protected QuestState state = QuestState.Unknown;

    public UnityEvent OnQuestStateUpdate;

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
        OnQuestStateUpdate.Invoke();
    }
}

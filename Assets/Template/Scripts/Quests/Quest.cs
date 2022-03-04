using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class Quest : MonoBehaviour
{
    protected QuestState State = QuestState.Unknown;

    [FormerlySerializedAs("OnQuestStateUpdate")] public UnityEvent onQuestStateUpdate;

    public enum QuestState
    {
        Unknown, Unaccepted, Started, Failed, Completed
    }

    public QuestState GetProgress()
    {
        return State;
    }

    public void SetProgress(QuestState newState)
    {
        State = newState;
        onQuestStateUpdate.Invoke();
    }

    public abstract string GetQuestName();
    public abstract string GetQuestDetail();
}

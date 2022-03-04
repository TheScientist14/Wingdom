using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTreesQuest : Quest
{
    [SerializeField] BubbleSpeech acceptedQuestEndDialog;

    private BrokenTreeBehaviour[] _brokenTrees;

    // Start is called before the first frame update
    void Start()
    {
        _brokenTrees = FindObjectsOfType<BrokenTreeBehaviour>();
        foreach (BrokenTreeBehaviour brokenTree in _brokenTrees)
        {
            brokenTree.onHeal.AddListener(UpdateQuestState);
        }
        DialogManager.Instance.onBubbleShown.AddListener(StartQuestOnDialog);
    }

    void UpdateQuestState()
    {
        bool treesHaveBeenHealed = true;
        foreach(BrokenTreeBehaviour brokenTree in _brokenTrees)
        {
            if (!brokenTree.HasBeenHealed())
            {
                treesHaveBeenHealed = false;
                break;
            }
        }
        if (treesHaveBeenHealed)
        {
            EndQuest();
        }
    }

    public void StartQuest()
    {
        SetProgress(QuestState.Started);
        foreach(BrokenTreeBehaviour brokenTree in _brokenTrees)
        {
            brokenTree.SetCanBeHealed(true);
            brokenTree.onHeal.AddListener(UpdateQuestState);
        }
    }

    public void EndQuest()
    {
        SetProgress(QuestState.Completed);
    }
    
    public void StartQuestOnDialog(BubbleSpeech shownBubble)
    {
        if (shownBubble.Equals(acceptedQuestEndDialog)){
            StartQuest();
            DialogManager.Instance.onBubbleShown.RemoveListener(StartQuestOnDialog);
        }
    }

    public override string GetQuestName()
    {
        return "Wounded trees";
    }
    
    public override string GetQuestDetail()
    {
        return "The royal delegation injured trees during their stay. Vanias, the deity of the forest want you to heal " +
               "them thank to a potion she gave you. You must heal four trees." +
               " Come back to see her when it is done";
    }
}

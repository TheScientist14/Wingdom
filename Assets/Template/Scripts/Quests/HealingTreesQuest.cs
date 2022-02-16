using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTreesQuest : Quest
{
    [SerializeField] BubbleSpeech acceptedQuestEndDialog;

    private BrokenTreeBehaviour[] brokenTrees;

    // Start is called before the first frame update
    void Start()
    {
        brokenTrees = FindObjectsOfType<BrokenTreeBehaviour>();
        foreach (BrokenTreeBehaviour brokenTree in brokenTrees)
        {
            brokenTree.onHeal.AddListener(UpdateQuestState);
        }
    }

    void UpdateQuestState()
    {
        bool treesHaveBeenHealed = true;
        foreach(BrokenTreeBehaviour brokenTree in brokenTrees)
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
        foreach(BrokenTreeBehaviour brokenTree in brokenTrees)
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
            DialogManager.instance.onBubbleShown.RemoveListener(StartQuestOnDialog);
        }
    }
}

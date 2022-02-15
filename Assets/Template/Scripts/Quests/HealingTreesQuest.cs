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
    }

    void Update()
    {
        if (DialogManager.instance.GetLastBubble() != null && DialogManager.instance.GetLastBubble().Equals(acceptedQuestEndDialog) && (state == QuestState.Unknown || state == QuestState.Unaccepted))
        {
            StartQuest();
        }
    }

    IEnumerator UpdateQuestState()
    {
        while (true)
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
            yield return new WaitForSeconds(1);
        }
    }

    public override void StartQuest()
    {
        base.StartQuest();
        foreach(BrokenTreeBehaviour brokenTree in brokenTrees)
        {
            brokenTree.SetCanBeHealed(true);
        }
        StartCoroutine("UpdateQuestState");
    }

    public override void EndQuest()
    {
        state = QuestState.Completed;
        StopCoroutine("UpdateQuestState");
    }
}

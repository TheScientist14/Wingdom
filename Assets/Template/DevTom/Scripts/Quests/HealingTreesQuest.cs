using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTreesQuest : Quest
{
    private BrokenTreeBehaviour[] brokenTrees;

    // Start is called before the first frame update
    void Start()
    {
        brokenTrees = FindObjectsOfType<BrokenTreeBehaviour>();
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
        StartCoroutine("UpdateQuestState");
    }

    public override void EndQuest()
    {
        state = QuestState.Completed;
        StopCoroutine("UpdateQuestState");
    }
}

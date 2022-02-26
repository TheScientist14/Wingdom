using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TimmyBehaviour : MonoBehaviour
{
    [SerializeField] Monolog dialog;
    [SerializeField] Interactable interactable;
    [SerializeField] Quest findTimmyQuest;
    [SerializeField] Quest takeTimmyHomeQuest;
    [SerializeField] NavMeshAgent navAgent;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (interactable)
        {
            interactable.AddAction(Talk);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        findTimmyQuest.OnQuestStateUpdate.AddListener(FollowPlayer);
        takeTimmyHomeQuest.OnQuestStateUpdate.AddListener(Vanish);
    }

    void Talk()
    {
        DialogManager.instance.StartDialog(dialog);
    }

    void FollowPlayer()
    {
        if (findTimmyQuest.getProgress() == Quest.QuestState.Completed)
        {
            StartCoroutine(TrackPlayer());
        }
    }

    IEnumerator TrackPlayer()
    {
        while (true)
        {
            navAgent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Vanish()
    {
        if (takeTimmyHomeQuest.getProgress() == Quest.QuestState.Completed)
        {
            Destroy(gameObject);
        }
    }

}

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

    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        if (interactable)
        {
            interactable.AddAction(Talk);
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        findTimmyQuest.onQuestStateUpdate.AddListener(FollowPlayer);
        takeTimmyHomeQuest.onQuestStateUpdate.AddListener(Vanish);
    }

    void Talk()
    {
        DialogManager.Instance.StartDialog(dialog);
    }

    void FollowPlayer()
    {
        if (findTimmyQuest.GetProgress() == Quest.QuestState.Completed)
        {
            StartCoroutine(TrackPlayer());
        }
    }

    IEnumerator TrackPlayer()
    {
        while (true)
        {
            navAgent.SetDestination(_player.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Vanish()
    {
        if (takeTimmyHomeQuest.GetProgress() == Quest.QuestState.Completed)
        {
            Destroy(gameObject);
        }
    }

}

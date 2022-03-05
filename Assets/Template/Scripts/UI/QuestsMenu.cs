using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestsMenu : MonoBehaviour
{
    [SerializeField] GameObject questsMenuPanel;
    [SerializeField] GameObject firstSelectedButton;
    [SerializeField] TextMeshProUGUI questName1;
    [SerializeField] TextMeshProUGUI questName2;
    [SerializeField] TextMeshProUGUI questName3;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] Button buttonQuest1;
    [SerializeField] Button buttonQuest2;
    [SerializeField] Button buttonQuest3;

    private string _questDescriptionTmp1;
    private string _questDescriptionTmp2;
    private string _questDescriptionTmp3;

    private Quest[] _quests;
    
    // Start is called before the first frame update
    void Start()
    {
        questsMenuPanel.SetActive(false);
        questName1.text = null;
        questName2.text = null;
        questName3.text = null;

        buttonQuest1.interactable = false;
        buttonQuest2.interactable = false;
        buttonQuest3.interactable = false;

        questDescription.text = null;
        _questDescriptionTmp1 = null;
        _questDescriptionTmp2 = null;
        _questDescriptionTmp3 = null;

        _quests = FindObjectsOfType<Quest>();

        foreach (Quest quest in _quests)
        {
            quest.onQuestStateUpdate.AddListener(UpdateStartedQuest);
        }
    }

    public void OnShowHide()
    {
        questsMenuPanel.SetActive(!questsMenuPanel.activeSelf);
        if (questsMenuPanel.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            GameManager.Instance.Pause();
        }
        else
        {
            GameManager.Instance.Resume();
        }
    }

    public void UpdateStartedQuest()
    {
        questName1.text = null;
        questName2.text = null;
        questName3.text = null;

        buttonQuest1.interactable = false;
        buttonQuest2.interactable = false;
        buttonQuest3.interactable = false;

        questDescription.text = null;
        _questDescriptionTmp1 = null;
        _questDescriptionTmp2 = null;
        _questDescriptionTmp3 = null;

        foreach (Quest quest in _quests)
        {
            if (quest.GetProgress() == Quest.QuestState.Started)
            {
                if (questName1.text == null)
                {
                    questName1.text = quest.GetQuestName();
                    _questDescriptionTmp1 = quest.GetQuestDetail();
                    buttonQuest1.interactable = true;
                }
                else if (questName2.text == null)
                {
                    questName2.text = quest.GetQuestName();
                    _questDescriptionTmp2 = quest.GetQuestDetail();
                    buttonQuest2.interactable = true;
                }
                else if (questName3.text == null)
                {
                    questName3.text = quest.GetQuestName();
                    _questDescriptionTmp3 = quest.GetQuestDetail();
                    buttonQuest3.interactable = true;
                }
            }
        }
    }

    public void OnClickDetail1()
    {
        questDescription.text = _questDescriptionTmp1;
    }

    public void OnClickDetail2()
    {
        questDescription.text = _questDescriptionTmp2;
    }

    public void OnClickDetail3()
    {
        questDescription.text = _questDescriptionTmp3;
    }
}
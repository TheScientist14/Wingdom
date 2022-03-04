using TMPro;
using UnityEditor.Search;
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
    
    private string questDescriptionTmp1;
    private string questDescriptionTmp2;
    private string questDescriptionTmp3;

    private Quest[] quests;

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
        questDescriptionTmp1 = null;
        questDescriptionTmp2 = null;
        questDescriptionTmp3 = null;

        quests =  FindObjectsOfType<Quest>();

        foreach (Quest quest in quests)
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
        /*foreach (Quest quest in quests)
        {
            if (quest.GetProgress() == Quest.QuestState.Started)
            {
                if (questName1.text != null)
                {
                    Debug.Log("questName1 != null");
                    if (questName2.text == null && questName3.text == null)
                    {
                        questName2.text = quest.GetQuestName();
                        questDescriptionTmp2 = quest.GetQuestDetail();
                        buttonQuest2.interactable = true;
                    }
                    else if (questName2.text != null && questName3.text == null)
                    {
                        questName3.text = quest.GetQuestName();
                        questDescriptionTmp3 = quest.GetQuestDetail();
                        buttonQuest3.interactable = true;
                    }
                    else if (questName2.text == null && questName3.text != null)
                    {
                        questName2.text = questName3.text;
                        questDescriptionTmp2 = questDescriptionTmp3;

                        questName3.text = quest.GetQuestName();
                        questDescriptionTmp3 = quest.GetQuestDetail();

                        buttonQuest2.interactable = true;
                        buttonQuest3.interactable = true;
                    }
                }
                else if (questName1.text == null)
                {
                    Debug.Log("questName1 == null");
                    if (questName2.text == null && questName3.text == null)
                    {
                        questName1.text = quest.GetQuestName();
                        questDescriptionTmp1 = quest.GetQuestDetail();
                        buttonQuest1.interactable = true;
                    }
                    else if (questName2.text != null && questName3.text == null)
                    {
                        questName1.text = questName2.text;
                        questDescriptionTmp1 = questDescriptionTmp2;

                        questName2.text = quest.GetQuestName();
                        questDescriptionTmp2 = quest.GetQuestDetail();

                        buttonQuest1.interactable = true;
                        buttonQuest2.interactable = true;
                    }
                    else if (questName2.text == null && questName3.text != null)
                    {
                        questName1.text = questName3.text;
                        questDescriptionTmp1 = questDescriptionTmp3;

                        questName2.text = quest.GetQuestName();
                        questDescriptionTmp2 = quest.GetQuestDetail();

                        questName3.text = null;
                        questDescriptionTmp3 = null;

                        buttonQuest1.interactable = true;
                        buttonQuest2.interactable = true;
                        buttonQuest3.interactable = false;
                    }
                    else if (questName2.text != null && questName3.text != null)
                    {
                        questName1.text = questName2.text;
                        questDescriptionTmp1 = questDescriptionTmp2;

                        questName2.text = questName3.text;
                        questDescriptionTmp2 = questDescriptionTmp3;

                        questName3.text = quest.GetQuestName();
                        questDescriptionTmp3 = quest.GetQuestDetail();

                        buttonQuest1.interactable = true;
                        buttonQuest2.interactable = true;
                        buttonQuest3.interactable = true;
                    }
                }
            }

            else if (quest.GetProgress() == Quest.QuestState.Completed)
            {
                if (quest.GetQuestName() == questName1.text)
                {
                    if (questName2.text == null && questName3.text == null)
                    {
                        questName1.text = null;
                        buttonQuest1.interactable = false;
                    }
                    else if (questName2.text != null && questName3 == null)
                    {
                        questName1.text = questName2.text;
                        questName2.text = null;
                        buttonQuest2.interactable = false;
                    }
                    else if (questName2.text != null && questName3 != null)
                    {
                        questName1.text = questName2.text;
                        questDescriptionTmp1 = questDescriptionTmp2;
                        
                        questName2.text = questName3.text;
                        questDescriptionTmp2 = questDescriptionTmp3;
                        
                        questName3.text = null;
                        questDescriptionTmp3 = null;

                        buttonQuest3.interactable = false;
                    }
                }
                else if (quest.GetQuestName() == questName2.text)
                {
                    if (questName3.text == null)
                    {
                        questName2.text = null;
                        questDescriptionTmp2 = null;
                        buttonQuest2.interactable = false;
                    }
                    else if (questName3 != null)
                    {
                        questName2.text = questName3.text;
                        questDescriptionTmp2 = questDescriptionTmp3;
                        
                        questName3.text = null;
                        questDescriptionTmp3 = null;
                        
                        buttonQuest3.interactable = false;
                    }
                }
                else if (quest.GetQuestName() == questName3.text)
                {
                    questName3.text = null;
                    questDescriptionTmp3 = null;
                    
                    buttonQuest3.interactable = false;
                }
            }
        }*/
    }

    public void OnClickDetail1()
    {
        questDescription.text = questDescriptionTmp1;
    }
    
    public void OnClickDetail2()
    {
        questDescription.text = questDescriptionTmp2;
    }
    
    public void OnClickDetail3()
    {
        questDescription.text = questDescriptionTmp3;
    }
}

using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestsMenu : MonoBehaviour
{

    [SerializeField] GameObject questsMenuPanel;
    [SerializeField] GameObject firstSelectedButton;
    [SerializeField] TextMeshProUGUI questName1;
    [SerializeField] TextMeshProUGUI questName2;
    [SerializeField] TextMeshProUGUI questName3;
    [SerializeField] TextMeshProUGUI questDescription;

    private string questDescriptionTmp1;
    private string questDescriptionTmp2;
    private string questDescriptionTmp3;

    private Quest[] quests;

    // Start is called before the first frame update
    void Start()
    {
        questsMenuPanel.SetActive(false);
        questName1 = null;
        questName2 = null;
        questName3 = null;

        questDescription = null;
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
        foreach (Quest quest in quests)
        {
            if (questName1 != null && questName2 != null && questName3 == null)
            {
                questName3.text = quest.GetQuestName();
                questDescriptionTmp1 = quest.GetQuestDetail();
            }
            else if (questName1 != null && questName2 == null)
            {
                if (questName3 == null)
                {
                    questName2.text = quest.GetQuestName();
                    questDescriptionTmp2 = quest.GetQuestDetail();
                }
                else
                {
                    questName2.text = questName3.text;
                    questDescriptionTmp2 = questDescriptionTmp3;
                    
                    questName3.text = quest.GetQuestName();
                    questDescriptionTmp3 = quest.GetQuestDetail();
                }
            }
            else if (questName1 == null)
            {
                if (questName2 == null && questName3 == null)
                {
                    questName1.text = quest.GetQuestName();
                    questDescriptionTmp1 = quest.GetQuestDetail();
                }
                else if (questName2 != null && questName3 == null)
                {
                    questName1.text = questName2.text;
                    questDescriptionTmp1 = questDescriptionTmp2;
                    
                    questName2.text = quest.GetQuestName();
                    questDescriptionTmp2 = quest.GetQuestDetail();
                }
                else if (questName2 == null && questName3 != null)
                {
                    questName1.text = questName3.text;
                    questDescriptionTmp1 = questDescriptionTmp3;
                    
                    questName2.text = quest.GetQuestName();
                    questDescriptionTmp2 = quest.GetQuestDetail();

                    questName3 = null;
                    questDescriptionTmp3 = null;
                    questDescription = null;
                }
                else if (questName2 != null && questName3 != null)
                {
                    questName1.text = questName2.text;
                    questDescriptionTmp1 = questDescriptionTmp2;
                    
                    questName2.text = questName3.text;
                    questDescriptionTmp2 = questDescriptionTmp3;
                        
                    questName3.text = quest.GetQuestName();
                    questDescriptionTmp3 = quest.GetQuestDetail();
                }
            }
        }
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

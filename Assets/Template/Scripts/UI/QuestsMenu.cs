using TMPro;
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

    private Quest[] quests;

    // Start is called before the first frame update
    void Start()
    {
        questsMenuPanel.SetActive(false);

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
            
        }
    }
    
}

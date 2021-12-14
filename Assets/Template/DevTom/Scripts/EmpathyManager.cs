using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmpathyManager : MonoBehaviour
{
    [SerializeField] GameObject journal;
    [SerializeField] Image pnjIcon;
    [SerializeField] TextMeshProUGUI pnjName;
    [SerializeField] TextMeshProUGUI pnjDescription;
    [SerializeField] TextMeshProUGUI empathyStatDisplayer;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject prevButton;
    private int pnjIndex = 0;

    private Dictionary<Speaker, int> pnjEmpathy;

    public static EmpathyManager instance;

    public Speaker[] tests;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pnjEmpathy = new Dictionary<Speaker, int>();
        foreach (Speaker test in tests)
        {
            RegisterPNJ(test);
        }
        Debug.Log(pnjEmpathy);
        journal.SetActive(false);
    }

    // empathy managing
    public void RegisterPNJ(Speaker pnj)
    {
        pnjEmpathy[pnj] = 0;
    }

    public void RegisterPNJ(Speaker pnj, int initEmpathy)
    {
        pnjEmpathy[pnj] = initEmpathy;
    }

    public void UpdateEmpathyPNJ(Speaker pnj, int empathyDelta)
    {
        pnjEmpathy[pnj] += empathyDelta;
    }

    public int GetEmpathyPNJ(Speaker pnj)
    {
        return pnjEmpathy[pnj];
    }

    // journal displaying
    void OnJournal()
    {
        journal.SetActive(!journal.activeSelf);
        UpdateJournalPage();
    }

    public void CloseJournal()
    {
        journal.SetActive(false);
    }

    public void NextPage()
    {
        if(pnjIndex < pnjEmpathy.Count)
        {
            pnjIndex++;
            UpdateJournalPage();
        }
    }

    public void PrevPage()
    {
        if (pnjIndex > 0)
        {
            pnjIndex--;
            UpdateJournalPage();
        }
    }

    void UpdateJournalPage()
    {
        Dictionary<Speaker, int>.KeyCollection.Enumerator pnjs = pnjEmpathy.Keys.GetEnumerator();
        pnjs.MoveNext();
        for(int i = 0; i < pnjIndex; i++)
        {
            if (!pnjs.MoveNext())
            {
                return;
            }
        }
        pnjDescription.text = pnjs.Current.journalDescription;
        pnjName.text = pnjs.Current.name;
        pnjIcon.sprite = pnjs.Current.icon;
        empathyStatDisplayer.text = GetEmpathyPNJ(pnjs.Current).ToString();
        nextButton.SetActive(pnjs.MoveNext());
        prevButton.SetActive(pnjIndex > 0);
    }
}

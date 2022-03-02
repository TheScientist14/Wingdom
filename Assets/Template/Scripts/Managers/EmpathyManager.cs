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
    private int _pnjIndex = 0;

    private Dictionary<Speaker, int> _pnjEmpathy;

    public static EmpathyManager Instance;

    public Speaker[] tests;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _pnjEmpathy = new Dictionary<Speaker, int>();
        foreach (Speaker test in tests)
        {
            RegisterPnj(test);
        }
        Debug.Log(_pnjEmpathy);
        journal.SetActive(false);
    }

    // empathy managing
    public void RegisterPnj(Speaker pnj)
    {
        RegisterPnj(pnj, 0);
    }

    public void RegisterPnj(Speaker pnj, int initEmpathy)
    {
        _pnjEmpathy.Add(pnj, initEmpathy);
    }

    public void UpdateEmpathyPnj(Speaker pnj, int empathyDelta)
    {
        _pnjEmpathy[pnj] += empathyDelta;
    }

    public int GetEmpathyPnj(Speaker pnj)
    {
        return _pnjEmpathy[pnj];
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
        if(_pnjIndex < _pnjEmpathy.Count)
        {
            _pnjIndex++;
            UpdateJournalPage();
        }
    }

    public void PrevPage()
    {
        if (_pnjIndex > 0)
        {
            _pnjIndex--;
            UpdateJournalPage();
        }
    }

    void UpdateJournalPage()
    {
        Dictionary<Speaker, int>.KeyCollection.Enumerator pnjs = _pnjEmpathy.Keys.GetEnumerator();
        pnjs.MoveNext();
        for(int i = 0; i < _pnjIndex; i++)
        {
            if (!pnjs.MoveNext())
            {
                return;
            }
        }
        pnjDescription.text = pnjs.Current.journalDescription;
        pnjName.text = pnjs.Current.name;
        pnjIcon.sprite = pnjs.Current.icon;
        empathyStatDisplayer.text = GetEmpathyPnj(pnjs.Current).ToString();
        nextButton.SetActive(pnjs.MoveNext());
        prevButton.SetActive(_pnjIndex > 0);
    }
}

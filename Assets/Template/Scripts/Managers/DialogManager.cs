using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] Image speakerIcon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject dialogsContainer;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] GameObject nextButton;
    [SerializeField] TextMeshProUGUI[] answersText;
    [SerializeField] Speaker player;

    private BubbleSpeech _currentBubble;
    private BubbleSpeech _lastBubble;

    [System.Serializable]
    public class OnBubbleShown : UnityEvent<BubbleSpeech> { };
    public OnBubbleShown onBubbleShown;

    public static DialogManager Instance;

    void Awake()
    {
        if (Instance == null)
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
        dialogsContainer.SetActive(false);
        foreach(GameObject answerButton in answerButtons)
        {
            answerButton.SetActive(false);
        }
    }

    public void StartDialog(BubbleSpeech conversation)
    {
        _currentBubble = conversation;
        dialogsContainer.SetActive(true);
        GameManager.Instance.SetInputsActive(false);
        UpdateBubble();
    }

    private void EndDialog()
    {
        GameManager.Instance.SetInputsActive(true);
        dialogsContainer.SetActive(false);
    }

    public void Next()
    {
        if (!text.isTextOverflowing)
        {
            if (_currentBubble.nextBubble != null)
            {
                _lastBubble = _currentBubble;
                _currentBubble = _currentBubble.nextBubble;
                UpdateBubble();
            }
            else
            {
                EndDialog();
            }
        }
        else
        {
            text.text = text.text.Substring(text.firstOverflowCharacterIndex);
        }
    }

    public void Answer(int i)
    {
        Answer chosenAnswer = ((Answers)_currentBubble).answers[i];
        foreach (Answer.PnjEmpathyImpact pnjEmpathyImpact in chosenAnswer.pnjEmpathyImpacts)
        {
            EmpathyManager.Instance.UpdateEmpathyPnj(pnjEmpathyImpact.pnj, pnjEmpathyImpact.empathyImpact);
        }
        text.gameObject.SetActive(true);
        nextButton.SetActive(true);
        foreach(GameObject answerButton in answerButtons)
        {
            answerButton.SetActive(false);
        }
        _lastBubble = _currentBubble;
        _currentBubble = chosenAnswer;
        onBubbleShown.Invoke(_currentBubble);
        Next();
    }

    private void UpdateBubble()
    {
        if(_currentBubble is Answers)
        {
            DisplayAnswers();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(nextButton);
            text.text = _currentBubble.text;
            speakerIcon.sprite = _currentBubble.speaker.icon;
            onBubbleShown.Invoke(_currentBubble);
            // speaker name
        }
    }

    private void DisplayAnswers()
    {
        // currentBubble is Answers
        text.gameObject.SetActive(false);
        nextButton.SetActive(false);
        speakerIcon.sprite = player.icon;
        Answer[] answers = ((Answers)_currentBubble).answers;
        for(int i = 0; i < answers.Length; i++)
        {
            answerButtons[i].SetActive(true);
            answersText[i].text = answers[i].text;
        }
        EventSystem.current.SetSelectedGameObject(answerButtons[0]);
    }

    public BubbleSpeech GetLastBubble()
    {
        return _lastBubble;
    }
}

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

    private BubbleSpeech currentBubble;
    private BubbleSpeech lastBubble;

    [System.Serializable]
    public class OnBubbleShown : UnityEvent<BubbleSpeech> { };
    public OnBubbleShown onBubbleShown;

    public static DialogManager instance;

    void Awake()
    {
        if (instance == null)
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
        dialogsContainer.SetActive(false);
        foreach(GameObject answerButton in answerButtons)
        {
            answerButton.SetActive(false);
        }
    }

    public void StartDialog(BubbleSpeech conversation)
    {
        currentBubble = conversation;
        dialogsContainer.SetActive(true);
        GameManager.instance.SetInputsActive(false);
        updateBubble();
    }

    private void EndDialog()
    {
        GameManager.instance.SetInputsActive(true);
        dialogsContainer.SetActive(false);
    }

    public void Next()
    {
        if (!text.isTextOverflowing)
        {
            if (currentBubble.nextBubble != null)
            {
                lastBubble = currentBubble;
                currentBubble = currentBubble.nextBubble;
                updateBubble();
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
        Answer chosenAnswer = ((Answers)currentBubble).answers[i];
        foreach (Answer.PnjEmpathyImpact pnjEmpathyImpact in chosenAnswer.pnjEmpathyImpacts)
        {
            EmpathyManager.instance.UpdateEmpathyPNJ(pnjEmpathyImpact.pnj, pnjEmpathyImpact.empathyImpact);
        }
        text.gameObject.SetActive(true);
        nextButton.SetActive(true);
        foreach(GameObject answerButton in answerButtons)
        {
            answerButton.SetActive(false);
        }
        lastBubble = currentBubble;
        currentBubble = chosenAnswer;
        onBubbleShown.Invoke(currentBubble);
        Next();
    }

    private void updateBubble()
    {
        if(currentBubble is Answers)
        {
            displayAnswers();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(nextButton);
            text.text = currentBubble.text;
            speakerIcon.sprite = currentBubble.speaker.icon;
            onBubbleShown.Invoke(currentBubble);
            // speaker name
        }
    }

    private void displayAnswers()
    {
        // currentBubble is Answers
        text.gameObject.SetActive(false);
        nextButton.SetActive(false);
        speakerIcon.sprite = player.icon;
        Answer[] answers = ((Answers)currentBubble).answers;
        for(int i = 0; i < answers.Length; i++)
        {
            answerButtons[i].SetActive(true);
            answersText[i].text = answers[i].text;
        }
        EventSystem.current.SetSelectedGameObject(answerButtons[0]);
    }

    public BubbleSpeech GetLastBubble()
    {
        return lastBubble;
    }
}

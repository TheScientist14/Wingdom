using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    [SerializeField] Image speakerIcon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI overflownText;
    [SerializeField] GameObject dialogsContainer;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] GameObject nextButton;
    [SerializeField] TextMeshProUGUI[] answersText;

    private Conversation currentConversation;
    private int eventIndex = -1;
    private bool needsToShowAnswers = false;
    private PlayerInput[] inputs;

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
        inputs = FindObjectsOfType<PlayerInput>();
        foreach(GameObject answerButton in answerButtons)
        {
            answerButton.SetActive(false);
        }
    }

    public void StartDialog(Conversation conversation)
    {
        currentConversation = conversation;
        eventIndex = -1;
        dialogsContainer.SetActive(true);
        foreach(PlayerInput input in inputs)
        {
            input.enabled = false;
        }
        Next();
    }

    private void EndDialog()
    {
        foreach (PlayerInput input in inputs)
        {
            input.enabled = true;
        }
        dialogsContainer.SetActive(false);
    }

    public void Next()
    {
        if (!text.isTextOverflowing)
        {
            if (needsToShowAnswers)
            {
                displayAnswers();
            }
            else
            {
                nextBubble();
            }
        }
        else
        {
            text.text = text.text.Substring(text.firstOverflowCharacterIndex);
        }
    }

    public void Answer(int i)
    {
        // TODO : implement empathy impact
        Debug.Log("empathy : +" + ((Question)currentConversation.bubbleSpeeches[eventIndex]).answers[i].empathyImpact);
        text.gameObject.SetActive(true);
        nextButton.SetActive(true);
        foreach(GameObject answerButton in answerButtons)
        {
            answerButton.SetActive(false);
        }
        needsToShowAnswers = false;
        Next();
    }

    private void nextBubble()
    {
        if (eventIndex + 1 < currentConversation.bubbleSpeeches.Length)
        {
            eventIndex++;
            overflownText.text = "";
            text.text = currentConversation.bubbleSpeeches[eventIndex].text;
            speakerIcon.sprite = currentConversation.bubbleSpeeches[eventIndex].speaker.icon;
            if (currentConversation.bubbleSpeeches[eventIndex] is Question)
            {
                needsToShowAnswers = true;
            }
        }
        else
        {
            EndDialog();
        }
    }

    private void displayAnswers()
    {
        text.gameObject.SetActive(false);
        nextButton.SetActive(false);
        speakerIcon.sprite = currentConversation.player.icon;
        Answer[] answers = ((Question)currentConversation.bubbleSpeeches[eventIndex]).answers;
        for(int i = 0; i < answers.Length; i++)
        {
            answerButtons[i].SetActive(true);
            answersText[i].text = answers[i].answer;
        }
    }
}

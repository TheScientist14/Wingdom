using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/BubbleSpeech/Question")]
public class Question : BubbleSpeech
{
    public Answer[] answers;
}

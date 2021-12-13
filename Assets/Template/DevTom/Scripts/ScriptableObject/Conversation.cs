using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "ScriptableObjects/Conversation")]
public class Conversation : ScriptableObject
{
    public BubbleSpeech[] bubbleSpeeches;
    public Speaker player;
}
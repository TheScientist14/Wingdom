using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BubbleSpeech : ScriptableObject
{
    [Multiline] public string text;
    public Speaker speaker;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer", menuName = "ScriptableObjects/BubbleSpeech/Answer")]
public class Answer : BubbleSpeech
{
    [Serializable]
    public struct PnjEmpathyImpact
    {
        public Speaker pnj;
        public int empathyImpact;
    }

    public PnjEmpathyImpact[] pnjEmpathyImpacts;
}

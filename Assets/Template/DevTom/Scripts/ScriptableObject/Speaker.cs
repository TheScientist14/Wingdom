using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker", menuName = "ScriptableObjects/Speaker")]
public class Speaker : ScriptableObject
{
    public new string name;
    public Sprite icon;
    [Multiline] public string journalDescription;
}

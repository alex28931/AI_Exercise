using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FC_Param", menuName = "Database/DialogueSystem/DialogueEntry")]
public class DialogueEntry : ScriptableObject
{

    [SerializeField]
    private uint dialogueID;
    [SerializeField]
    private uint dialogueEntry;
    [SerializeField]
    private string dialogueContent;
    [SerializeField]
    private int nextEntry;

}

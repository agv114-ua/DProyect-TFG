using System;
using UnityEngine;

/**
 * Separa los datos de la lógica a través de un ScriptableObject
 * */

[CreateAssetMenu (menuName = "NPC/Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("NPC info")]
    public string NPCName;
    public Sprite icon;

    [Header("Dialogue Lines")]
    [TextArea] public string greeting;
    public DialogueLine[] conversation;
    [TextArea] public string farewell;
   
}

[Serializable]
public class DialogueLine
{
    [TextArea] public string text;
}

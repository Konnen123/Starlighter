using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcFile",menuName = "NpcFilesArchives")]
public class NPC : ScriptableObject
{
    public string npcName;
    [TextArea(3, 15)] public string[] npcDialogue;
    [TextArea(3, 15)] public string[] playerDialogue;
}

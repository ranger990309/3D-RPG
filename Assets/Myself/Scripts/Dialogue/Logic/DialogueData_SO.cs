using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue",menuName = "Dialogue/Dialogue Data")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        dialogueIndex.Clear();
        foreach (var item in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(item.ID))
            {
                dialogueIndex.Add(item.ID, item);
            }
        }
    }
#endif

    public QuestData_SO GetQuest()
    {
        QuestData_SO currentQuest=null;
        foreach (var item in dialoguePieces)
        {
            if (item.quest != null)
            {
                currentQuest = item.quest;
            }
        }
        return currentQuest;
    }
}

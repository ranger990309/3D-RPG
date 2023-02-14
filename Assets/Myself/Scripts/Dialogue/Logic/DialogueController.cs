using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    bool canTalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
    }
    private void Update() 
    {
        if (canTalk && Input.GetMouseButtonDown(1))
        {
            OpenDialogue();
        }
    }
    private void OpenDialogue()
    {
        //��UI���
        //����Ի�������Ϣ
        DialogueUI.Instance.UpdataDialogueData(currentData);
        DialogueUI.Instance.UpdataMainDialogue(currentData.dialoguePieces[0]); 
    }
}

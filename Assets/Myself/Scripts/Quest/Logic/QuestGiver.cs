using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;
    QuestData_SO currentData;

    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishDialogue;

    #region 获得任务状态
    public bool IsStarted
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentData))
            {
                return QuestManager.Instance.GetTask(currentData).IsStarted;
            }
            else return false;
        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentData))
            {
                return QuestManager.Instance.GetTask(currentData).IsComplete;
            }
            else return false;
        }
    }
    public bool IsFinish
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentData))
            {
                return QuestManager.Instance.GetTask(currentData).IsFinished;
            }
            else return false;
        }
    }

    #endregion

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    private void Start()
    {
        controller.currentData = startDialogue;
        currentData = controller.currentData.GetQuest();
    }
    private void Update()
    {
        if (IsStarted)
        {
            if (IsComplete)
            {
                controller.currentData = completeDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }
        if (IsFinish)
        {
            controller.currentData = finishDialogue;
        }
    }
}

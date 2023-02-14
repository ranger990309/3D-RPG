using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemTooltip tooltip;
    private bool isOpen;
    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameButton questNameButton;
    [Header("Text Content")]
    public Text questContentText;
    [Header("Requirement")]
    public RectTransform requireTransform;
    public QuestRequirement requirement;
    [Header("Reward Panel")]
    public RectTransform rewardTransform;
    public ItemUI rewardUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentText.text = string.Empty;
            //œ‘ æ√Ê∞Â
            SetupQuestList();

            if (!isOpen)
            {
                tooltip.gameObject.SetActive(false);
            }
        }
    }
    public void SetupQuestList()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetupNameButton(item.questData);
            newTask.questContentText = questContentText;
        }
    }

    public void SetupRequireList(QuestData_SO questData)
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in questData.questRequires)
        {
            var quest = Instantiate(requirement, requireTransform);
            if (questData.isFinished)
            {
                quest.SetupRequirement(item.name, questData.isFinished);
            }
            else
            quest.SetupRequirement(item.name, item.requireAmount, item.currentAmount);
        }
    }

    public void SetupRewardItem(ItemData_SO itemData,int amount)
    {
        var item = Instantiate(rewardUI, rewardTransform);
        item.SetupItemUI(itemData, amount);
    }
}

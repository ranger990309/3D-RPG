using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemUI currentItemUI;
    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestUI.Instance.tooltip.gameObject.SetActive(true);
        QuestUI.Instance.tooltip.SetupTooltip(currentItemUI.currentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.tooltip.gameObject.SetActive(false);
    }
}

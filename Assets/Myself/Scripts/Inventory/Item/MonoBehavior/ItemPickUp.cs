using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //将物品添加进入背包
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //装备武器并销毁Scene里的物品
            //GameManager.Instance.playerStates.EquipWeapon(itemData);
            QuestManager.Instance.UpdataQuestProgress(itemData.itemName, itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}

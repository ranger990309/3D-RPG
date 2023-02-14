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
            //����Ʒ��ӽ��뱳��
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //װ������������Scene�����Ʒ
            //GameManager.Instance.playerStates.EquipWeapon(itemData);
            QuestManager.Instance.UpdataQuestProgress(itemData.itemName, itemData.itemAmount);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Useable,Weapon,Armor}
[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item Data")]

public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    /// <summary>
    /// ¿É·ñ¶Ñµþ
    /// </summary>
    public bool stackable;
    public UseableItems_SO useableItemsData;

    [TextArea]
    public string description = "";

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
    public AnimatorOverrideController weaponAnimator;
}

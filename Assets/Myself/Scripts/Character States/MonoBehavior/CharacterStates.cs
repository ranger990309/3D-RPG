using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类会记录玩家的各种状态
/// 例如：生命值相关，攻击防御相关等基础数据
/// </summary>
public class CharacterStates : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    public CharacterData_SO templateCharacterDeta;
    public AttackData_SO templateAttackDeta;
    public AttackData_SO baseAttackData;
    private RuntimeAnimatorController baseAnimator;

    [Header("Weapon")]
    public Transform weaponSlot;

    [HideInInspector]
    public bool isCritical;

    private void Awake()
    {
        if (templateCharacterDeta != null)
        {
            characterData = Instantiate(templateCharacterDeta);
        }
        if (templateAttackDeta != null)
        {
            attackData = Instantiate(templateAttackDeta);
        }
        baseAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }
    #region Read From Data_SO
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            else return 0;
        }
        set
        {
            characterData.maxHealth = value;
        }
    }
    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentHealth;
            }
            else return 0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }
    public int BaseDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.baseDefence;
            }
            else return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }
    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentDefence;
            }
            else return 0;
        }
        set
        {
            characterData.currentDefence= value;
        }
    }
    #endregion

    #region Character Combat

    public void TakeManage(CharacterStates attacker,CharacterStates defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdataExp(characterData.killPoint);
        }
    }
    public void TakeManage(int damage,CharacterStates defener)
    {
        int currentDamage = Mathf.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        GameManager.Instance.playerStates.characterData.UpdataExp(characterData.killPoint);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage+1);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }

    #endregion

    #region Equip Weapon

    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }
    public void EquipWeapon(ItemData_SO weapon)
    {
        if(weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
        }
        //更新属性
        attackData.ApplyWeaponData(weapon.weaponData);
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
    }

    public void UnEquipWeapon()
    {
        if(weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        attackData.ApplyWeaponData(baseAttackData);
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;

    }

    #endregion

    #region

    public void ApplyHealth(int amount)
    {
        if (CurrentHealth + amount <= MaxHealth)
            CurrentHealth += amount;
        else
            CurrentHealth = MaxHealth;
    }

    #endregion
}

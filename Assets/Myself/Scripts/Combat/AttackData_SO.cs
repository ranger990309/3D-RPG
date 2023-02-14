using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName = "Character States/Attack")]

public class AttackData_SO : ScriptableObject
{
    public float attackRange;  //½üÕ½¹¥»÷·¶Î§
    public float skillRange;  //Ô¶³Ì¹¥»÷·¶Î§
    public float coolDown;   //¹¥»÷ÀäÈ´
    public int minDamage;   
    public int maxDamage;
    public float criticalMultiplier;  //±©»÷¼Ó³É
    public float criticalChance;   //±©»÷ÂÊ

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }
}

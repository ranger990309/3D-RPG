using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack",menuName = "Character States/Attack")]

public class AttackData_SO : ScriptableObject
{
    public float attackRange;  //��ս������Χ
    public float skillRange;  //Զ�̹�����Χ
    public float coolDown;   //������ȴ
    public int minDamage;   
    public int maxDamage;
    public float criticalMultiplier;  //�����ӳ�
    public float criticalChance;   //������

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

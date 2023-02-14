using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt :EnemyController
{
    public QuestData_SO quest;
    [Header("Skill")]
    public float kickForce = 10f;//击飞的力度

    public void KickOff()//用于击飞目标的代码
    {
        if (attackTarget!=null&&transform.IsFacingTarget(attackTarget.transform))
        {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
    private void OnDestroy()
    {
        QuestManager.Instance.GetTask(quest).questData.isComplete = true;
        QuestManager.Instance.GetTask(quest).questData.questRequires[0].currentAmount = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt :EnemyController
{
    public QuestData_SO quest;
    [Header("Skill")]
    public float kickForce = 10f;//���ɵ�����

    public void KickOff()//���ڻ���Ŀ��Ĵ���
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

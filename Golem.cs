using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25f;

    public GameObject rockPrefab;
    public Transform handPos;
    public void RockKickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();

            transform.LookAt(attackTarget.transform);
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            targetStates.GetComponent<NavMeshAgent>().isStopped=true;
            targetStates.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            targetStates.TakeManage(characterStates, targetStates);
        }
    }
    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}

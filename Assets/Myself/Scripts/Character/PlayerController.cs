using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;
    private float lastAttackTime;
    private CharacterStates characterStates;
    private bool isDeath;
    private float stopDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();  //����GetComponent<T>()��ȡ����ΪT�����
        anim = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
        stopDistance = agent.stoppingDistance;
    }

    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }

    private void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        GameManager.Instance.RigisterPlayer(characterStates);
    }

    private void OnDisable()
    {
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    private void Update()
    {
        isDeath = (characterStates.CurrentHealth == 0);
        if (isDeath)
        {
            GameManager.Instance.NotifyObservers();
        }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDeath);
    }
    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();//ֹͣ�ýű������صĶ��������Я�̣�����ֹͣ��ҵĵ�ǰ�����ж���
        if (isDeath) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;  //NavMeshAgent.destination��NavmeshAgent��Ŀ���
    }
    private void EventAttack(GameObject target)
    {
        if (isDeath) return;
        if (target != null)
        {
            attackTarget = target;
            characterStates.isCritical = UnityEngine.Random.value < characterStates.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }
    IEnumerator MoveToAttackTarget()
    {
        //move
        agent.isStopped = false;
        agent.stoppingDistance = characterStates.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStates.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        //attack
        if(lastAttackTime < 0)
        {
            anim.SetBool("Critical", characterStates.isCritical);
            anim.SetTrigger("Attack");
            //������ȴʱ��
            lastAttackTime = characterStates.attackData.coolDown;
        }
    }

    //Animation Event
    private void Hit()//������������ڹ����Animation��Attack������ĳһ֡��,���е���һ֡�ʹ����ⷽ��
    {
        if (attackTarget.CompareTag("Rock"))
        {
            if (attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one+Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);
            }
        }
        else 
        {
        var targetStates = attackTarget.GetComponent<CharacterStates>();
        targetStates.TakeManage(characterStates, targetStates);
        }
    }
}

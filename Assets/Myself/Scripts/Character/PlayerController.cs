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
        agent = GetComponent<NavMeshAgent>();  //函数GetComponent<T>()获取类型为T的组件
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
        StopAllCoroutines();//停止该脚本所挂载的对象的所有携程，例如停止玩家的当前的所有动作
        if (isDeath) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;  //NavMeshAgent.destination是NavmeshAgent的目标点
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
            //重置冷却时间
            lastAttackTime = characterStates.attackData.coolDown;
        }
    }

    //Animation Event
    private void Hit()//这个方法附着在怪物的Animation的Attack动画的某一帧里,运行到那一帧就触发这方法
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

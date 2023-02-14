using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]


public class EnemyController : MonoBehaviour, IEndGameObserver
{
    private NavMeshAgent agent;
    private EnemyStates enemyStates;
    private Animator anim;
    protected GameObject attackTarget;
    private Vector3 wayPoint;
    protected CharacterStates characterStates;
    private Quaternion guardRotation;
    private Collider colliders;
    private float speed;
    private bool isWalk;
    private bool isChase;
    private bool isRun;
    private bool isDeath;
    private bool playerDeath = false;
    private float lastAttackTime;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;//巡逻范围
    private Vector3 guardPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
        colliders = GetComponent<Collider>();
        remainLookAtTime = lookAtTime;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        speed = agent.speed;
    }
    private void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //FIXME:Scene切换后需修改
        GameManager.Instance.AddObserver(this);
    }
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}
    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);
        if (GetComponent<LootSpawner>() && isDeath)
        {
            GetComponent<LootSpawner>().Spawnloot();
        }
        if (QuestManager.IsInitialized && isDeath)
        {
            QuestManager.Instance.UpdataQuestProgress(this.name, 1);
        }
    }
    private void Update()
    {
        if (characterStates.CurrentHealth == 0)
        {
            isDeath = true;
        }
        if (!playerDeath)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    private void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Run", isRun);
        anim.SetBool("Critical", characterStates.isCritical);
        anim.SetBool("Death", isDeath);
    }
    private void SwitchStates()
    {
        if (isDeath)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                isRun = false;
                isChase = false;
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                }
                break;
            case EnemyStates.PATROL:
                Patroling();
                break;
            case EnemyStates.CHASE:
                ChaseTarget();
                break;
            case EnemyStates.DEAD:
                Deadth();
                break;
        }
    }
    private void Patroling()
    {
        isChase = false;
        agent.speed = speed * 0.5f;
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                GetNewWayPoint();
            }
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var item in colliders)
        {
            if (item.CompareTag("Player"))
            {
                attackTarget = item.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    private void ChaseTarget()
    {
        isWalk = false;
        isChase = true;
        agent.speed = speed;
        if (!FoundPlayer())
        {
            isRun = false;
            if (remainLookAtTime > 0)
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (!isGuard)
            { enemyStates = EnemyStates.PATROL; }
            else
            { enemyStates = EnemyStates.GUARD; }
        }
        else
        {
            isRun = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
            remainLookAtTime = lookAtTime;
        }
        //如果在攻击范围则进行攻击
        if (TargetInAttackRange() || TargetInSkillRange())
        {
            isRun = false;
            agent.isStopped = true;
            if (lastAttackTime < 0)
            {
                lastAttackTime = characterStates.attackData.coolDown;//冷却攻击
                //暴击判断(随机数小于就暴击)
                characterStates.isCritical = (Random.value < characterStates.attackData.criticalChance);
                //执行攻击
                Attack();
            }
        }
    }
    private void Attack()//攻击的动画控制
    {
        transform.LookAt(attackTarget.transform);//看向Player
        if (TargetInAttackRange())
        {
            //近战动画
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            //远程动画
            anim.SetTrigger("Skill");
        }
    }

    private bool TargetInAttackRange()//近战
    {
        if (attackTarget != null)
        {
            return (Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStates.attackData.attackRange);
        }
        else return false;
    }

    private bool TargetInSkillRange()//远距离攻击
    {
        if (attackTarget != null)
        {
            return (Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStates.attackData.skillRange);
        }
        else return false;
    }

    private void GetNewWayPoint()//得到下个新巡查地点
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }
    private void Deadth()
    {
        colliders.enabled = false;
        agent.radius = 0;
        Destroy(gameObject, 2f);
    }
    public void EndNotify()
    {
        playerDeath = true;
        anim.SetBool("Win", true);
        isChase = false;
        isWalk = false;
        isRun = false;
        attackTarget = null;
        //获胜动画

        //停止移动
        //关闭Agent
    }

    //Animation Event
    private void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeManage(characterStates, targetStates);
        }
    }


}

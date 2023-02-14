using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer,HitEnemy,HitNothing}
    public RockStates rockStates;
    private Rigidbody rb;
    private Vector3 direction;

    [Header("Basic Setting")]
    public float force;
    public GameObject target;
    public int damage;
    public GameObject breakEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one + Vector3.one;
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude<1f)
        {
            rockStates = RockStates.HitNothing;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    collision.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    collision.gameObject.GetComponent<CharacterStates>().TakeManage(damage, collision.gameObject.GetComponent<CharacterStates>());

                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (collision.gameObject.GetComponent<Golem>())
                {
                    var otherStates = collision.gameObject.GetComponent<CharacterStates>();
                    otherStates.TakeManage(damage, otherStates);
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemryScript : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    private AiStates.State state;

    private int curPatrol = 0;

    private int patrolCount;

    private bool attackLoop = false;

    public Transform[] patrolRoute;

    public float distanceThreshold;

    public float maxPlayerDist = 10f;

    public float maxAttackDist = 2f;

    public float distanceWhenAggro = 0.2f;

    public Transform playerTr;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        patrolCount = patrolRoute.Length - 1;

        if (patrolCount > 0)
        {
            state = AiStates.State.Patroling;
            agent.SetDestination(patrolRoute[curPatrol].position);
        }
        else if(patrolCount <= 0)
        {
            state = AiStates.State.Aggro;
        }
    }

    private void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerTr.position, transform.position);
        switch (state)
        {
            case AiStates.State.Patroling:
                if (patrolCount + 1 > 0)
                {
                    if (agent.remainingDistance < distanceThreshold)
                    {
                        IncreaseCurPatrol();
                        agent.SetDestination(patrolRoute[curPatrol].position);
                    }
                }
                break;

            case AiStates.State.Aggro:
                agent.SetDestination(playerTr.position);
                if (distanceFromPlayer <= maxAttackDist)
                {
                    state = AiStates.State.Attacking;
                    animator.SetTrigger("attackTrigger");
                    animator.SetBool("attack", true);
                    attackLoop = true;
                }
                break;
            case AiStates.State.Attacking:
                attackLoop = true;
                if (distanceFromPlayer > maxAttackDist)
                {
                    state = AiStates.State.Aggro;
                    attackLoop = false;
                }
                break;
            default:
                break;
        }

        if ((state != AiStates.State.Aggro || state != AiStates.State.Attacking) && distanceFromPlayer < maxPlayerDist && !attackLoop)
        {
            state = AiStates.State.Aggro;
            agent.stoppingDistance = distanceWhenAggro;
        }
        else if(distanceFromPlayer > maxPlayerDist && !attackLoop)
        {
            state = AiStates.State.Patroling;
            agent.stoppingDistance = 0;
        }

        

    }

    void FixedUpdate()
    {
        animator.SetFloat("vel_x", Mathf.Clamp(agent.velocity.x, -1, 1));
        animator.SetFloat("vel_y", Mathf.Clamp(agent.velocity.z, -1, 1));
        animator.SetBool("attack", attackLoop);
    }

    private void IncreaseCurPatrol()
    {
        curPatrol++;
        if (curPatrol > patrolRoute.Length - 1)
        {
            curPatrol = 0;
        }
    }
}

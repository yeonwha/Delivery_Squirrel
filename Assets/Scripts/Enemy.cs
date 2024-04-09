using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;  
    [SerializeField] Transform target;    // a reference to the object we want to follow
    private enum EnemyState { IDLE, CHASE };    // possible states for the Enemy NPC
    private EnemyState state;                   // current state

    private float distanceToTarget = float.MaxValue;    // distance to player - default to far away
    private float chaseRange = 10f;                     // when target is closer than this, chase!
    private float attackRange = 2.0f;

    void SetState(EnemyState newState) {
        state = newState;
    }

    private void Start() {
        SetState(EnemyState.IDLE);          // start off in an idle state
    }

    void Update_Idle() {
        agent.isStopped = true;                             // start the agent
        if (distanceToTarget <= chaseRange) {
            SetState(EnemyState.CHASE);
        }
    }
    void Update_Chase() {
        agent.isStopped = false;                            // stop the agent
        agent.SetDestination(target.transform.position);    // follow the target
        if( distanceToTarget > chaseRange) {
            SetState(EnemyState.IDLE);
        }
    }

    void Update() {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        // what happens in Update depends on what state we're currently in!
        switch (state) {    
            case EnemyState.IDLE:   Update_Idle(); break;
            case EnemyState.CHASE:  Update_Chase(); break;
        }
    }

 

    private void OnDrawGizmos()  {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);  // draw a circle to show chase range
       Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // draw a circle to show chase range
    }
}


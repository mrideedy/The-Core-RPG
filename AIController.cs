using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    
{
    [SerializeField] float chaseDistance = 7f;
    [SerializeField] float suspicionTime = 5f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 3f;
    [Range(0,1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;
        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;
        Vector3 guardPosition;
        float timeSInceLastSawPlayer = Mathf.Infinity;
        float timeSInceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = GetComponent<Health>();
        fighter = GetComponent <Fighter>();
        mover = GetComponent<Mover>();
        guardPosition = transform.position;
    }

    private void Update()
     {
        if(health.IsDead()) return;

        if( InAttackRangeOfPlayer()  && fighter.CanAttack(player))
        {
         
         AttackBehaviour();
        }

        else if (timeSInceLastSawPlayer < suspicionTime)
        {
            SuspicionBehaviour();
        }

        else
        {
          PatrolBehaviour();
        }
        UpdateTimers();
   }
    private void UpdateTimers()
    {
        timeSInceLastSawPlayer += Time.deltaTime;
        timeSInceArrivedAtWaypoint += Time.deltaTime;
    }
    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition;
        if (patrolPath != null)
        {
            if (AtWayPoint())
            {
                timeSInceArrivedAtWaypoint = 0;
                CycleWayPoint();
            }
            nextPosition = GetCurrentWayPoint();
        }
        if (timeSInceArrivedAtWaypoint > waypointDwellTime)
        {
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }
        
    }
    private bool AtWayPoint()
    {
        float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        // throw new NotImplementedException();
        return distanceToWayPoint < waypointTolerance;
    }
    private void CycleWayPoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }
    private Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }
    private void AttackBehaviour()
    {
        timeSInceLastSawPlayer = 0;
        fighter.Attack(player);
    }

    private void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    
    
    private bool InAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < chaseDistance;
    }

    private void OnDrawGizmosSelected()
    {
       Gizmos.color = Color.blue;
       Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
    }
}

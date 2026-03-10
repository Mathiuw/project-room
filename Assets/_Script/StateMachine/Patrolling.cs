using UnityEngine;
using UnityEngine.AI;

public class Patrolling : IState
{
    private readonly Path _path;
    private readonly EnemyAi _enemyAi;
    private readonly NavMeshAgent _navMeshAgent;

    private int waypointIndex = 0;
    private Vector2 desiredPosition;

    Collider[] rangeCheck;

    public Patrolling(EnemyAi enemyAi, Path path, NavMeshAgent navMeshAgent)
    {
        _enemyAi = enemyAi;
        _path = path;
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _navMeshAgent.SetDestination(_path.Waypoints[waypointIndex]);
        desiredPosition = new Vector2(_path.Waypoints[waypointIndex].x, _path.Waypoints[waypointIndex].z);
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        ViewCheck();
        WalkPath();
    }

    private void WalkPath() 
    {
        Transform enemyTransform = _enemyAi.transform;
        Vector2 position = new Vector2(enemyTransform.position.x, enemyTransform.position.z);
        
        if (position != desiredPosition) return;

        waypointIndex++;

        if (waypointIndex == _path.Waypoints.Length) waypointIndex = 0;

        desiredPosition = new Vector2(_path.Waypoints[waypointIndex].x, _path.Waypoints[waypointIndex].z);
        _navMeshAgent.SetDestination(_path.Waypoints[waypointIndex]);
    }

    private void ViewCheck() 
    {
        Transform enemyTransform = _enemyAi.transform;

        rangeCheck = Physics.OverlapSphere(enemyTransform.position, _enemyAi.Radius, _enemyAi.TargetMask);

        if (rangeCheck.Length == 0) return;

        foreach (Collider collider in rangeCheck)
        {
            Transform target = collider.transform;
            Vector3 directionToTarget = (target.position - enemyTransform.position).normalized;
            float distanceToTarget = Vector3.Distance(enemyTransform.position, target.position);

            if (Vector3.Angle(enemyTransform.forward, directionToTarget) < +_enemyAi._angle / 2)
            {
                if (!Physics.Raycast(enemyTransform.position, directionToTarget, distanceToTarget, _enemyAi.ObstructionMask))
                {
                    if (_enemyAi.Target == null && target.CompareTag("Player"))
                    {
                        _enemyAi.Target = target;
                        Debug.Log("Enemy got target");
                        break;
                    }
                }
            }
        }
    }
}

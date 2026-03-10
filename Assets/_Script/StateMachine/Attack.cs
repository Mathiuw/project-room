using UnityEngine.AI;

public class Attack : IState
{
    private readonly EnemyAi _enemyAi;
    private readonly NavMeshAgent _navMeshAgent;

    public Attack(EnemyAi enemyAi, NavMeshAgent navMeshAgent) 
    {  
        _enemyAi = enemyAi; 
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _navMeshAgent.SetDestination(_enemyAi.transform.position);
        _enemyAi.StartShooting();
    }

    public void OnExit()
    {
        _enemyAi.StopShooting();
    }

    public void Tick()
    {
        _enemyAi.LookToTarget();
    }
}

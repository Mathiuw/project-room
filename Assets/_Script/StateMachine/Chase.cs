using UnityEngine;
using UnityEngine.AI;

public class Chase : IState
{
    private readonly EnemyAi _enemyAi;
    private readonly NavMeshAgent _navMeshAgent;

    public Chase(EnemyAi enemyAi, NavMeshAgent navMeshAgent) 
    {
        _enemyAi = enemyAi;
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _enemyAi.Run(true);
    }

    public void OnExit()
    {
        _enemyAi.Run(false);
    }

    public void Tick()
    {
        _navMeshAgent.destination = _enemyAi.Target.position;
    }
}

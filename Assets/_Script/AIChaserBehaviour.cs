using MaiNull.Player;
using UnityEngine;
using UnityEngine.AI;

namespace MaiNull
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIChaserBehaviour : MonoBehaviour
    {
        NavMeshAgent agent;
        Transform target;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            target = FindFirstObjectByType<PlayerMovement>()?.transform;
        }

        private void Update()
        {
            if (target)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}

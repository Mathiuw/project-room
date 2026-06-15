using UnityEngine;
using UnityEngine.AI;

namespace MaiNull
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIChaserBehaviour : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _target;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _target = FindFirstObjectByType<Player>()?.transform;
        }

        private void Update()
        {
            if (_target)
            {
                _agent.SetDestination(_target.position);
            }
        }
    }
}

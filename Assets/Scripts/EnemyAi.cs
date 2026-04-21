using System;
using MaiNull.StateMachines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace MaiNull
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAi : MonoBehaviour
    {
        [Header("AI settings")]
        [SerializeField] private float baseSpeed = 6;
        [SerializeField] private float runningSpeedMultiplier = 1.4f;
        
        [Header("Attack")]
        [SerializeField] private Transform shootRaycastTransform;
        [SerializeField] private float attackRange = 10f;
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstInterval = 1f;

        [FormerlySerializedAs("_path")]
        [Header("Patrolling")]
        [SerializeField] private Path path;
        [Header("Field of view")]
        [Range(0, 360)] public float angle = 160;
        public float Radius { get; } = 20;
        [field: SerializeField] public LayerMask TargetMask { get; private set; }
        [field: SerializeField] public LayerMask ObstructionMask { get; private set; }

        public Transform Target { get; set; }

        private NavMeshAgent _navMeshAgent;
        private StateMachine _stateMachine;

        private void OnDestroy() => StopAllCoroutines();

        private void Awake()
        {
            if (!path)
            {
                Debug.LogError("Enemy doesnt have path");
                enabled = false;
                return;
            }

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = baseSpeed;
        }

        private void Start()
        {
            _stateMachine = new StateMachine();

            Patrolling patrolling = new(this, path, _navMeshAgent);
            Chase chase = new (this, _navMeshAgent);
            Attack attack = new (this, _navMeshAgent);

            At(patrolling, chase, HasTarget());
            At(chase, attack, IsInTargetReach());
            At(attack, chase, IsNotInTargetReach());

            _stateMachine.SetState(patrolling);
            return;

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            Func<bool> HasTarget() => () => Target != null;
            Func<bool> IsInTargetReach() => () => Vector3.Distance(transform.position, Target.position) < attackRange /*&& CanSeeTarget()*/;
            Func<bool> IsNotInTargetReach() => () => Vector3.Distance(transform.position, Target.position) > attackRange;
        }

        private void OnBodyPartHit(float resultDamage, Transform damageInstigator)
        {
            if (!Target)
            {
                Target = damageInstigator;
            }
        }

        private void Update()
        {
            _stateMachine?.Tick();
        }

        public void Run(bool value)
        {
            if (value)
            {
                _navMeshAgent.speed = baseSpeed * runningSpeedMultiplier;
            }
            else _navMeshAgent.speed = baseSpeed;
        }

        public void StartShooting()
        {
            
        }
        
        public void StopShooting()
        {
            
        }
        
        public bool CanSeeTarget()
        {
            Vector3 directionToTarget = (Target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, Target.position);

            return !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask);
        }

        public void LookToTarget()
        {
            transform.LookAt(Target);
        }

        public void Dead()
        {
            StopAllCoroutines();
            _navMeshAgent.enabled = false;
            GetComponentInChildren<Ragdoll>().SetRagdollState(true);
            enabled = false;
        }
    }
}
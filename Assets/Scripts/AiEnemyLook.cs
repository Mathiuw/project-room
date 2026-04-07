using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace MaiNull
{
    public class AiEnemyLook : MonoBehaviour
    {
        [SerializeField] Transform IKTarget;   
        [Range(0f, 1f)][SerializeField] float IkBodyWeight;
        [SerializeField] MultiAimConstraint IKBody;
        [Range(0f, 1f)][SerializeField] float IkHeadWeight;
        [SerializeField] MultiAimConstraint IKHead;

        EnemyAi enemyAi;

        void Awake()
        {
            enemyAi = GetComponent<EnemyAi>();   
        }

        void Update()
        {
            LookAtTarget();
        }

        void LookAtTarget() 
        {
            if (enemyAi.Target)
            {
                IKBody.weight = IkBodyWeight;
                IKHead.weight = IkHeadWeight;

                IKTarget.position = enemyAi.Target.position;
            }
            else 
            {
                IKBody.weight = 0;
                IKHead.weight = 0;
            } 
        }
    }
}

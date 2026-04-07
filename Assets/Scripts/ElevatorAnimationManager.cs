using UnityEngine;

namespace MaiNull
{
    [RequireComponent(typeof(Animator))]
    public class ElevatorAnimationManager : MonoBehaviour
    {
        Animator animator;

        void Awake() => animator = GetComponent<Animator>();

        void Start() 
        {
            SetState(false);
            SetPanelEvents();
        }

        void SetState(bool b) => animator.SetBool("open", b);

        void SetPanelEvents() 
        {
            ElevatorPanel[] interactions = GetComponentsInChildren<ElevatorPanel>();

            foreach (ElevatorPanel interaction in interactions)
            {
                interaction.onButtomPress += InvertState;
            }
        }

        void InvertState() 
        {
            bool state = animator.GetBool("open");

            animator.SetBool("open", !state);
        }
    }
}
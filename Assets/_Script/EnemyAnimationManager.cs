using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationManager : MonoBehaviour
{
    Animator animator;

    void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    void Start() 
    {
        
    }

    void StartWalk() => animator.SetBool("walk", true);

    void StopWalk() => animator.SetBool("walk", false);

    void StartAim() => animator.SetBool("aim", true);

    void StopAim() => animator.SetBool("aim", false);

    void SpeedMultiplier() => animator.SetFloat("speed multiplier", 1f);
}

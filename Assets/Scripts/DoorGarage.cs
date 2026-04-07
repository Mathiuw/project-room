using UnityEngine;

namespace MaiNull
{
    public class DoorGarage : MonoBehaviour
    {    
        [field: SerializeField] public KeycardReader[] keycardReaders { get; private set; } = new KeycardReader[4];
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            SetKeycardReaders();    
        }

        void SetKeycardReaders() 
        {
            for (int i = 0; i < keycardReaders.Length; i++)
            {
                if (keycardReaders[i])
                {
                    keycardReaders[i].OnAcceptKeycard += CheckKeycardreaders;
                }
                else
                {
                    Debug.LogWarning("Array index " + i + " doesnt have keycard reader");
                }
            }
        }

        void CheckKeycardreaders()
        {
            Debug.Log("check if can open doors");

            for (int i = 0; i < keycardReaders.Length; i++)
            {
                if (keycardReaders[i].Used) continue;
                else 
                {
                    Debug.Log("Not all keycard readers are used");
                    return;
                } 
            }

            animator.Play("Open");
            Debug.Log("Gate opened");
        }
    }
}

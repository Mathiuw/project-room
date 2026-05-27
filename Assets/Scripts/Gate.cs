using System.Linq;
using UnityEngine;

namespace MaiNull
{
    public class Gate : MonoBehaviour
    {    
        public KeycardReader[] keycardReaders = new KeycardReader[4];
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SetKeycardReaders();    
        }

        private void SetKeycardReaders() 
        {
            for (int i = 0; i < keycardReaders.Length; i++)
            {
                if (keycardReaders[i])
                {
                    keycardReaders[i].OnAcceptKeycard += CheckKeycardReaders;
                }
                else
                {
                    Debug.LogWarning("Array index " + i + " doesnt have keycard reader");
                }
            }
        }

        private void CheckKeycardReaders()
        {
            // print("check if can open doors");
            
            if (keycardReaders.Any(reader => !reader.Used))
            {
                // print("Not all keycard readers are used");
                return;
            }

            _animator.Play("Open");
            print("Gate opened");
        }
    }
}

using UnityEngine;

namespace MaiNull
{
	public class Pickable : MonoBehaviour, IInteractable, IUIName
    {
        public virtual string readName => "Pickable";

        public virtual void Interact(Transform interactor)
        {

        }
    }
}
using MaiNull.Interact;
using UnityEngine;

namespace MaiNull.Item
{
	public class Pickable : MonoBehaviour, IInteractable, IUIName
    {
        public virtual string readName => "Pickable";

        public virtual void Interact(Transform interactor)
        {

        }
    }
}
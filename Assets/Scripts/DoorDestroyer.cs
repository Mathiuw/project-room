using UnityEngine;

namespace MaiNull
{
    public class DoorDestroyer : MonoBehaviour
    {
        [SerializeField] float force;

        void OnTriggerEnter(Collider collision)
        {
            if (collision == null) return;

            Door door;

            if (door = collision.transform.GetComponentInParent<Door>()) 
            {
                door.DestroyDoor(transform.forward, force);
            }
        }
    }
}
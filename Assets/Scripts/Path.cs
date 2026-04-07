using UnityEngine;

namespace MaiNull
{
    public class Path : MonoBehaviour
    {
        public Vector3[] Waypoints { get; private set; }

        private void Awake()
        {
            // Set path
            Waypoints = new Vector3[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                Waypoints[i] = transform.GetChild(i).position;
                Waypoints[i] = new Vector3(Waypoints[i].x, transform.position.y, Waypoints[i].z);
            }
        }

        private void OnDrawGizmos()
        {
            float sphereSize = 0.25f;
            Vector3 startPosition = transform.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in transform)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypoint.position, sphereSize);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }
    }
}

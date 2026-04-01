using System.Xml.Schema;
using UnityEngine;

namespace MaiNull.Assets._Script.Player
{
    public class MoveAndSlide : MonoBehaviour
    {
        [SerializeField] private int moveVelocity = 20;
        [SerializeField] private float maxSlopeAngle = 55f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -10, 0);
        [SerializeField] private int maxBounces = 5;
        [SerializeField] private float skinWidth = 0.0015f;
        [SerializeField] private LayerMask layerMask;
        Bounds bounds;

        private void Awake()
        {
            
        }

        private void Update()
        {
            Collider collider = GetComponent<Collider>();
            if (!collider) return;

            bounds = collider.bounds;
            bounds.Expand(-2 * skinWidth);
        }

        private void FixedUpdate()
        {
            
        }

        public void Move(Vector3 moveAmount)
        {
            moveAmount = CollideAndSlide(moveAmount, transform.position, 0, false, moveAmount);
            moveAmount = CollideAndSlide(gravity, transform.position, 0, false, gravity);
        }

        private Vector3 CollideAndSlide(Vector3 vel, Vector3 pos, int depth, bool gravityPass, Vector3 velInit)
        {
            if (depth >= maxBounces)
            {
                return Vector3.zero;
            }

            float dist = vel.magnitude + skinWidth;

            RaycastHit hit;

            if (Physics.SphereCast(pos, bounds.extents.x, vel.normalized, out hit, layerMask))
            {
                Vector3 snapTosurface = vel.normalized * (hit.distance - skinWidth);
                Vector3 leftover = vel - snapTosurface;
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (snapTosurface.magnitude <= skinWidth)
                {
                    snapTosurface = Vector3.zero;
                }

                // normal ground/ slope
                if (angle <= maxSlopeAngle)
                {
                    if (gravityPass)
                    {
                        return snapTosurface;
                    }
                    leftover = ProjectAndScale(leftover, hit.normal);
                }
                // wall or steep slope
                else
                {
                    float scale = 1 - Vector3.Dot(
                        new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
                        new Vector3(velInit.x , 0, velInit.z).normalized
                    );

                    leftover = ProjectAndScale(leftover, hit.normal) * scale;

                    //if (isGrounded && !gravityPass)
                    //{
                    //    leftover = ProjectAndScale(
                    //        new Vector3(leftover.x, 0, leftover.z),
                    //        new Vector3(hit.normal.x, 0, hit.normal.z)
                    //    ).normalized;
                    //    leftover *= scale;
                    //}
                    //else
                    //{
                    //    leftover = ProjectAndScale(leftover, hit.normal) * scale;
                    //}

                }

                float leftoverMagnitude = leftover.magnitude;
                leftover = Vector3.ProjectOnPlane(leftover, hit.normal).normalized;
                leftover *= leftoverMagnitude;

                return snapTosurface + CollideAndSlide(leftover, pos + snapTosurface, depth + 1, gravityPass, velInit);
            }

            return vel;
        }

        private Vector3 ProjectAndScale(Vector3 vec , Vector3 normal)
        {
            float magnitude = vec.magnitude;
            vec = Vector3.ProjectOnPlane(vec, normal).normalized;
            vec *= magnitude;
            return vec;
        }
    }
}
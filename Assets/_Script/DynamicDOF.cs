using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DynamicDOF : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] float focusSpeed = 7.5f;
    [SerializeField] LayerMask collsionMask;
    float maxDistance = 5f;
    float distance;
    PostProcessVolume postProcessVolume;
    DepthOfField dof;

    void Start()
    {
        postProcessVolume = FindFirstObjectByType<PostProcessVolume>();

        if (postProcessVolume)
        {
            postProcessVolume.profile.TryGetSettings(out dof);
        }
        else 
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, collsionMask))
        {
            distance = Vector3.Distance(transform.position, hit.point);
        }
        else distance = maxDistance;

        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, distance, focusSpeed * Time.deltaTime);
    }
}

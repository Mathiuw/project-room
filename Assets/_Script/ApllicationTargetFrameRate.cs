using UnityEngine;

namespace MaiNull
{
	public class ApllicationTargetFrameRate : MonoBehaviour
	{
        [SerializeField] private int targetFrameRate = 60;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
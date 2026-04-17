using UnityEngine;

namespace MaiNull.Utils
{
	public class TargetFrameRate : MonoBehaviour
	{
        [SerializeField] private int targetFrameRate = 60;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
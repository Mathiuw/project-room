using UnityEngine;

namespace MaiNull
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private Vector3 maxRotation;
        private Vector3 _rotation;

        [SerializeField] private float speed;
        [SerializeField] private float growthIntensity = 1;
        [SerializeField] private float decayIntensity = 1;

        private float _intensity = 0;
        private float _seedX;
        private float _seedY;
        private float _seedZ;

        private void Awake()
        {
            _seedX = Random.Range(-1000, 1000);
            _seedY = Random.Range(-1000, 1000);
            _seedZ = Random.Range(-1000, 1000);
        }

        private void Update()
        {
            // Debug Input
            //if (Input.GetKey(KeyCode.Space)) intensity += growthIntensity * Time.deltaTime;

            _intensity -= decayIntensity * Time.deltaTime;
            _intensity = Mathf.Clamp01(_intensity);

            float intensityExponential = _intensity * _intensity;
            float time = Time.time * speed;

            _rotation.x = intensityExponential * maxRotation.x * Mathf.PerlinNoise(_seedX, time);
            _rotation.y = intensityExponential * maxRotation.y * Mathf.PerlinNoise(_seedY, time);
            _rotation.z = intensityExponential * maxRotation.z * Mathf.PerlinNoise(_seedZ, time);

            transform.localRotation = Quaternion.Euler(_rotation);                                                                     
        }

        public void AddCameraShake(float intensity, float speed)
        {
            this._intensity = intensity;
            this.speed = speed;
        }
    }
}
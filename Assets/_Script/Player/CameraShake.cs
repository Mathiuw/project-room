using MaiNull.Item;
using UnityEngine;

namespace MaiNull
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] Vector3 maxRotaion;
        Vector3 rotation;

        [SerializeField] private float speed;
        [SerializeField] private float growthIntensity = 1;
        [SerializeField] private float decayIntensity = 1;

        float intensity = 0;
        float seedX;
        float seedY;
        float seedZ;

        PlayerWeaponHolder playerWeaponHolder;

        private void Awake()
        {
            seedX = Random.Range(-1000, 1000);
            seedY = Random.Range(-1000, 1000);
            seedZ = Random.Range(-1000, 1000);
        }


        private void OnEnable()
        {
            if (playerWeaponHolder)
            {
                playerWeaponHolder.OnWeaponPickup += OnWeaponPickup;
            }
        }


        void Start()
        {
            playerWeaponHolder = FindFirstObjectByType<PlayerWeaponHolder>();

            if (playerWeaponHolder)
            {
                playerWeaponHolder.OnWeaponPickup += OnWeaponPickup;
            }
        }

        
        private void OnDisable()
        {
            if (playerWeaponHolder)
            {
                if (playerWeaponHolder.CurrentWeapon != null)
                {
                    playerWeaponHolder.CurrentWeapon.OnWeaponShot -= OnWeaponShot;
                }

                playerWeaponHolder.OnWeaponPickup -= OnWeaponPickup;
            }
        }

        private void OnWeaponPickup(Weapon weapon)
        {
            weapon.OnWeaponShot += OnWeaponShot;
        }

        private void OnWeaponShot(Weapon weapon, RaycastHit hit)
        {
            float intensity = weapon.WeaponData.intensity;
            float speed = weapon.WeaponData.speed;

            AddCameraShake(intensity, speed);
        }

        void Update()
        {
            // Debug Input
            //if (Input.GetKey(KeyCode.Space)) intensity += growthIntensity * Time.deltaTime;

            intensity -= decayIntensity * Time.deltaTime;
            intensity = Mathf.Clamp01(intensity);

            float intensityExponential = intensity * intensity;
            float time = Time.time * speed;

            rotation.x = intensityExponential * maxRotaion.x * PerlinNoise(seedX, time);
            rotation.y = intensityExponential * maxRotaion.y * PerlinNoise(seedY, time);
            rotation.z = intensityExponential * maxRotaion.z * PerlinNoise(seedZ, time);

            transform.localRotation = Quaternion.Euler(rotation);                                                                     
        }

        float PerlinNoise(float x, float y)
        {
            return Mathf.PerlinNoise(x, y);
        }

        public void AddCameraShake(float intensity, float speed)
        {
            this.intensity = intensity;
            this.speed = speed;
        }
    }
}
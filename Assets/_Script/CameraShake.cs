using UnityEngine;

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

    PlayerWeaponInteraction playerWeaponInteraction;

    private void Awake()
    {
        seedX = Random.Range(-1000, 1000);
        seedY = Random.Range(-1000, 1000);
        seedZ = Random.Range(-1000, 1000);
    }

    void Start()
    {
        playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction)
        {
            playerWeaponInteraction.onWeaponShot += OnWeaponShot;
        }
    }

    private void OnDisable()
    {
        playerWeaponInteraction.onWeaponShot -= OnWeaponShot;
    }

    void Update()
    {
        // Debug Input
        if (Input.GetKey(KeyCode.Space)) intensity += growthIntensity * Time.deltaTime;

        intensity -= decayIntensity * Time.deltaTime;
        intensity = Mathf.Clamp01(intensity);

        float intensityExponential = intensity * intensity;
        float time = Time.time * speed;

        rotation.x = intensityExponential * maxRotaion.x * PerlinNoise(seedX, time);
        rotation.y = intensityExponential * maxRotaion.y * PerlinNoise(seedY, time);
        rotation.z = intensityExponential * maxRotaion.z * PerlinNoise(seedZ, time);

        transform.localRotation = Quaternion.Euler(rotation);
    }

    private void OnWeaponShot(Weapon weaponShot)
    {
        float intensity = weaponShot.SOWeapon.intensity;
        float speed = weaponShot.SOWeapon.speed;

        AddCameraShake(intensity, speed);
    }

    float PerlinNoise(float x, float y) 
    {
        return Mathf.PerlinNoise(x,y);
    }

    public void AddCameraShake(float intensity, float speed) 
    {
        this.intensity = intensity;
        this.speed = speed;
    }
}

using UnityEngine;

namespace MaiNull
{
    public class PickableWeaponPersistent : Pickable
    {
        [field: SerializeField] public Weapon Weapon { get; private set; }

        public override string readName => base.readName;

        [Header("Particles")]
        [SerializeField] private Transform muzzleFlashTransform;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private ParticleSystem blood;
        protected AudioSource gunSound;

        public Transform Owner { get; private set; }


        private void Awake()
        {
            gunSound = GetComponent<AudioSource>();

            // Set hold state to false
            SetHoldState(false);
        }

        public override void Interact(Transform interactor)
        {
            interactor.TryGetComponent(out WeaponHolder weaponHolder);

            if (weaponHolder)
            {
                weaponHolder.PickUpWeapon(Weapon);
            }
        }

        public void SetHoldState(bool holdState, Transform owner = null)
        {
            Owner = owner;

            Rigidbody rb = GetComponent<Rigidbody>();

            if (!rb) return;

            rb.isKinematic = holdState;

            if (holdState)
            {
                rb.interpolation = RigidbodyInterpolation.None;
            }
            else
            {
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                transform.SetParent(null);
            }

            Collider[] colliders = GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = holdState;
        }

        protected void PlayMuzzleFlashParticle()
        {
            Instantiate(muzzleFlash, muzzleFlashTransform.position, muzzleFlashTransform.rotation, transform);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace MaiNull.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIHit : MonoBehaviour
    {
        private AudioSource _hitSound;
        private CanvasGroup _canvasGroup;
        private WeaponHolder _playerWeaponInteraction;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;

            _hitSound = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _playerWeaponInteraction = FindFirstObjectByType<Player>()?.GetComponent<WeaponHolder>();

            if (_playerWeaponInteraction)
            {
                _playerWeaponInteraction.OnWeaponPickup += OnWeaponPickup;
            }
        }

        private void OnWeaponPickup(Weapon weapon)
        {
            weapon.OnWeaponShot += OnWeaponShot;
        }

        private void OnWeaponShot(Weapon weapon, RaycastHit hit)
        {
            if (hit.collider != null)
            {
                StartCoroutine(HitmarkerCoroutine());
            }
        }

        private IEnumerator HitmarkerCoroutine()
        {
            _canvasGroup.alpha = 1;
            _hitSound.Play();

            yield return new WaitForSeconds(_hitSound.clip.length);

            _canvasGroup.alpha = 0;
            yield break;
        }
    }
}
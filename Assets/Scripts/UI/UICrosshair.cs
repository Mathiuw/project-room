using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public class UICrosshair : MonoBehaviour
    {
        [SerializeField] private Sprite dotCrosshair;
        [SerializeField] private Image reloadCrosshair;
        private WeaponHolder _playerWeaponInteraction;
        private Image _crosshair;

        private void Awake()
        {
            _crosshair = GetComponent<Image>();

            reloadCrosshair.enabled = false;
        }

        private void Start()
        {
            _playerWeaponInteraction = FindFirstObjectByType<WeaponHolder>();

            if (_playerWeaponInteraction)
            {
                _playerWeaponInteraction.OnWeaponPickup += OnWeaponPickup;
                _playerWeaponInteraction.OnWeaponDrop += OnWeaponDrop;
                //playerWeaponInteraction.OnReloadStart += OnReloadStart;
            }

            SetCroshair(dotCrosshair);
        }

        private void OnDisable()
        {
            _playerWeaponInteraction.OnWeaponPickup -= OnWeaponPickup;
            _playerWeaponInteraction.OnWeaponDrop -= OnWeaponDrop;
            //playerWeaponInteraction.OnReloadStart -= OnReloadStart;
        }

        private void OnWeaponPickup(Weapon weapon)
        {
            SetCroshair(weapon.WeaponData.crosshair);
        }

        private void OnWeaponDrop()
        {
            SetCroshair(dotCrosshair);
        }

        private void OnReloadStart()
        {
            float reloadDuration = _playerWeaponInteraction.CurrentWeapon.WeaponData.reloadCooldown;

            StartCoroutine(ReloadLerp(reloadDuration));
        }

        private void SetCroshair(Sprite sprite)
        {
            _crosshair.sprite = sprite;

            if (!sprite)
            {
                _crosshair.enabled = false;
            }
            else
            {
                _crosshair.enabled = true;
            }
        }

        IEnumerator ReloadLerp(float duration)
        {
            Image ring = reloadCrosshair.GetComponent<Image>();

            _crosshair.enabled = false;
            reloadCrosshair.enabled = true;

            float timeElapsed = 0;

            ring.fillAmount = 0;

            while (ring.fillAmount < 1)
            {
                ring.fillAmount = Mathf.Lerp(0, 1f, timeElapsed / duration);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            reloadCrosshair.enabled = false;

            if (!_crosshair.sprite)
            {
                _crosshair.enabled = false;
            }
            else
            {
                _crosshair.enabled = true;
            }

            yield break;
        }
    }
}
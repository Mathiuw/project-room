using System.Collections;
using MaiNull.Item;
using MaiNull.Player;
using UnityEngine;

namespace MaiNull.UI
{
    [RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
    public class UIHit : MonoBehaviour
    {
        AudioSource hitSound;
        CanvasGroup canvasGroup;
        PlayerWeaponHolder playerWeaponInteraction;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

            hitSound = GetComponent<AudioSource>();
        }

        void Start()
        {
            playerWeaponInteraction = FindFirstObjectByType<Player.Player>().GetComponent<PlayerWeaponHolder>();

            if (playerWeaponInteraction)
            {
                playerWeaponInteraction.OnWeaponPickup += OnWeaponPickup;
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
            canvasGroup.alpha = 1;
            hitSound.Play();

            yield return new WaitForSeconds(hitSound.clip.length);

            canvasGroup.alpha = 0;
            yield break;
        }
    }
}
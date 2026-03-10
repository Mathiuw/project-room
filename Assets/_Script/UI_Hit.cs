using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class UI_Hit : MonoBehaviour
{
    AudioSource hitSound;
    CanvasGroup canvasGroup;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        hitSound = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerWeaponInteraction = FindFirstObjectByType<PlayerMovement>().GetComponent<PlayerWeaponInteraction>();

        if (playerWeaponInteraction)
        {
            playerWeaponInteraction.OnWeaponHit += OnWeaponHit;
        }
    }

    private void OnWeaponHit(RaycastHit hit)
    {
        StartCoroutine(HitmarkerCoroutine());
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

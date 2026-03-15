using MaiNull.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    Image crosshair;
    [SerializeField] Sprite dotCrosshair;
    [SerializeField] Image reloadCrosshair;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Awake()
    {
        crosshair = GetComponent<Image>();

        reloadCrosshair.enabled = false;
    }

    void Start() 
    {
        playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction)
        {
            playerWeaponInteraction.OnWeaponPickup += OnWeaponPickup;
            playerWeaponInteraction.OnWeaponDrop += OnWeaponDrop;
            playerWeaponInteraction.OnReloadStart += OnReloadStart;
        }

        SetCroshair(dotCrosshair);
    }

    private void OnDisable()
    {
        playerWeaponInteraction.OnWeaponPickup -= OnWeaponPickup;
        playerWeaponInteraction.OnWeaponDrop -= OnWeaponDrop;
        playerWeaponInteraction.OnReloadStart -= OnReloadStart;
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
        float reloadDuration = playerWeaponInteraction.Weapon.WeaponData.reloadTime;

        StartCoroutine(ReloadLerp(reloadDuration));
    }

    private void SetCroshair(Sprite sprite) 
    {
        crosshair.sprite = sprite;

        if (!sprite)
        {
            crosshair.enabled = false;
        }
        else
        {
            crosshair.enabled = true;
        }
    }

    IEnumerator ReloadLerp(float duration)
    {
        Image ring = reloadCrosshair.GetComponent<Image>();

        crosshair.enabled = false;
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

        if (!crosshair.sprite)
        {
            crosshair.enabled = false;
        }
        else
        {
            crosshair.enabled = true;
        }

        yield break;
    }
}

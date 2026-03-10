using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Keycards : MonoBehaviour
{
    [field: SerializeField] public float spriteOfset { get; private set; } = 10f;
    Inventory inventory;
    Image spriteTemplate;

    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform player = GameObject.FindWithTag("Player").transform;

        if (!player)
        {
            Debug.LogError("Cant find player");
            return;
        }

        inventory = player.GetComponent<Inventory>();

        if (!inventory)
        {
            Debug.LogError("Cant find inventory");
            return;
        }

        spriteTemplate = GetComponentInChildren<Image>();

        if (!spriteTemplate)
        {
            Debug.LogError("Cant find spriteTemplate image");
            return;
        }

        inventory.OnKeycardAdd += OnkeycardAdd;

        DrawKeycards();
    }

    private void OnDisable()
    {
        inventory.OnKeycardAdd -= OnkeycardAdd;
    }

    private void OnkeycardAdd(Keycard item)
    {
        DrawKeycards();
    }

    private void OnItemRemoved()
    {
        DrawKeycards();
    }

    private void DrawKeycards() 
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (inventory.keycards.Count == 0)
        {
            canvasGroup.alpha = 0;
            return;
        }

        int keycardcount = 0;
        float offfset = 0f;
        bool firstSprite = true;

        for (int i = 0; i < inventory.keycards.Count; i++)
        {
            if (inventory.keycards[i].PickableItemData.GetType() == typeof(KeycardData))
            {
                Sprite itemSprite = inventory.keycards[i].PickableItemData.hotbarSprite;

                if (!firstSprite)
                {
                    Vector2 spawnPosition = new Vector2(offfset, 0);

                    Image keycardSprite = Instantiate(spriteTemplate, Vector2.zero, Quaternion.identity, transform);
                    keycardSprite.rectTransform.anchoredPosition = spawnPosition;
                    keycardSprite.sprite = itemSprite;
                }
                else
                {
                    spriteTemplate.sprite = itemSprite;
                    firstSprite = false;
                }

                offfset += (spriteTemplate.rectTransform.rect.width + spriteOfset);
                keycardcount++;
            }
        }

        if (keycardcount != 0)
        {
            canvasGroup.alpha = 1;
        }
        else canvasGroup.alpha = 0;

        Debug.Log("Draw keycard inventory");
    }
}

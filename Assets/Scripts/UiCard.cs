using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull
{
    public class UiCard : MonoBehaviour
    {
        [SerializeField] private CardData cardDataData;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI tittle;
        [SerializeField] private TextMeshProUGUI description;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (!cardDataData) return;
            image.sprite = cardDataData.sprite;
            tittle.text = cardDataData.tittle;
            description.text = cardDataData.description;
        }
    }
}
using System;
using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "KeycardData", menuName = "Keycard/Keycard Data")]
    public class KeycardData : InventoryItemData
    {
        private enum EKeycardColor
        {
            Red,
            Green,
            Blue,
            Yellow
        }
        
        [SerializeField] private EKeycardColor keycardColor;
        public KeycardColorData colorData;

        public Material GetColorMaterial()
        {
            if (colorData)
                return keycardColor switch
                {
                    EKeycardColor.Red => colorData.redMaterial,
                    EKeycardColor.Green => colorData.greenMaterial,
                    EKeycardColor.Blue => colorData.blueMaterial,
                    EKeycardColor.Yellow => colorData.yellowMaterial,
                    _ => throw new ArgumentOutOfRangeException()
                };
            Debug.LogError("KeycardData does not contain color material");
            return null;
        }
    }
    
    [CreateAssetMenu(fileName = "KeycardColorData", menuName = "Keycard/Color Data")]
    public class KeycardColorData: ScriptableObject
    {
        public Material redMaterial;
        public Material greenMaterial;
        public Material blueMaterial;
        public Material yellowMaterial;
    }
}
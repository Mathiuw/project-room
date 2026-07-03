using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Player_Data", menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Health")]
        public int startMaxHealth;
        [Header("Progression")]
        public uint startLevel;
        public AnimationCurve experienceProgressionCurve;
        [Header("Input")]
        public InputActionReference moveInputAction;
        public InputActionReference lookInputAction;
        public InputActionReference jumpInputAction;
        public InputActionReference sprintInputAction;
        public InputActionReference interactInputAction;
        public InputActionReference attackInputAction;
        public InputActionReference reloadInputAction;
        public InputActionReference dropInputAction;
        public InputActionReference switchWeaponAction;
        public InputActionReference switchCardAction;
    }
}
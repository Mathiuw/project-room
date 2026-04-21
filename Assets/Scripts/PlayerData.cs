using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Player_Data", menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        public int maxHealth;
        public InputActionReference moveInputAction;
        public InputActionReference jumpInputAction;
        public InputActionReference sprintInputAction;
        public InputActionReference interactInputAction;
        public InputActionReference attackWeaponInputAction;
        public InputActionReference reloadWeaponInputAction;
        public InputActionReference dropWeaponInputAction;
    }
}
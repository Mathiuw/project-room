using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Player
{
    [CreateAssetMenu(fileName = "Player_Data", menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        public int maxHealth;
        public InputActionReference moveInputAction;
        public InputActionReference jumpInputAction;
        public InputActionReference sprintInputAction;
    }
}
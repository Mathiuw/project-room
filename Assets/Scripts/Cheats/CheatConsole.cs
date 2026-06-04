using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Cheats
{
	public class CheatConsole : MonoBehaviour
	{
		private bool _showConsole = false;
		private string _input;
		
		public static CheatConsole Instance;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void RuntimeInit()
		{
#if DEVELOPMENT_BUILD || UNITY_EDITOR
			LoadConsole();
#endif
		}
		private static void LoadConsole()
		{
			if (Instance != null)
				return;

			var newConsole = new GameObject { name = "[Cheat Console]" };
			Instance = newConsole.AddComponent<CheatConsole>();
			DontDestroyOnLoad(newConsole);

#if USE_INPUT_SYSTEM
            EnhancedTouchSupport.Enable();
#endif

			InputSystem.actions.FindAction("Toggle Console").started += Instance.OnToggleConsoleStarted;
			
			InputSystem.actions.FindActionMap("Debug").Enable();
		}
		
		private void OnToggleConsoleStarted (InputAction.CallbackContext obj)
		{
			_showConsole = !_showConsole;

			if (_showConsole) {
				InputSystem.actions.FindActionMap("Player").Disable();
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			} 
			else 
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				InputSystem.actions.FindActionMap("Player").Enable();
			}
		}

		private void OnDisable()
		{
			InputSystem.actions.FindActionMap("Debug").Disable();
		}

		private void OnGUI()
		{
			if (!_showConsole) return;

			float y = 0f;
			
			GUI.Box(new Rect(0, y, Screen.width, 30), "");
			GUI.backgroundColor = new Color(0, 0, 0, 0);
			
			_input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), _input);
		}

	}
}

using UnityEngine;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControl _playerControl;
        public static InputManager Instance { get; private set; }
        public bool APress { get; private set; }
        public bool ATap { get; private set; } = false;
        public bool BPress { get; private set; }
        public bool BTap { get; private set; } = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
            ReadInput();
        }


        private void ReadInput()
        {
            _playerControl = new PlayerControl();

            _playerControl.Gameplay.APress.Enable();
            _playerControl.Gameplay.APress.performed += _ => APress = true;
            _playerControl.Gameplay.APress.canceled += _ => APress = false;

            _playerControl.Gameplay.BPress.Enable();
            _playerControl.Gameplay.BPress.performed += _ => BPress = true;
            _playerControl.Gameplay.BPress.canceled += _ => BPress = false;
        }
    }
}
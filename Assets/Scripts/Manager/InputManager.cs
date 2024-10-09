using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public bool APress = false;
    public bool ATap = false;
    public bool BPress = false;
    public bool BTap = false;
    private PlayerControl _playerControl;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        ReadInput();
    }


    private void ReadInput()
    {
        _playerControl = new PlayerControl();

        _playerControl.Gameplay.APress.Enable();
        _playerControl.Gameplay.APress.performed += context => APress = true;
        _playerControl.Gameplay.APress.canceled += context => APress = false;

        _playerControl.Gameplay.BPress.Enable();
        _playerControl.Gameplay.BPress.performed += context => BPress = true;
        _playerControl.Gameplay.BPress.canceled += context => BPress = false;
    }
}

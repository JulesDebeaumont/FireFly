using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAction : MonoBehaviour
{
  public Player Player;

  private readonly float _rotationSpeed = 5f;
  private readonly float _runSpeedRatio = 7f;
  private readonly float _defaultMoveSpeed = 1f;

  private PlayerControl _playerControl;


  [SerializeField]
  private float _moveSpeed = 1f;

  [SerializeField]
  private Vector2 _leftStickInput;

  [SerializeField]
  private Vector2 _rightStickInput;

  [SerializeField]
  private float _playerAngle = 0f;


  private readonly PlayerState.EPlayerState[] FreeActionState = {
        PlayerState.EPlayerState.STAND,
        PlayerState.EPlayerState.WALK,
        PlayerState.EPlayerState.RUN
    };

  void Awake()
  {
    ReadInput();
  }

  void Update()
  {
    if (CanRotate())
    {
      Rotate();
    }
    // if (CanStand())
    // {
    //   Stand();
    // }
    if (CanMove())
    {
      Move();
    }
    if (CanRoll())
    {
      Roll();
    }
  }

  void FixedUpdate()
  {
    // TODO remttre le contenu de Update() ici une fois corrigé
    if (Player.PlayerState.State == PlayerState.EPlayerState.RUN || Player.PlayerState.State == PlayerState.EPlayerState.WALK)
    {
      MovePlayerByInputAndCamera();

    }
  }

  private void ReadInput()
  {
    _playerControl = new PlayerControl();

    _playerControl.Gameplay.LeftStick.Enable();
    _playerControl.Gameplay.LeftStick.performed += context =>
    {
      _leftStickInput = context.ReadValue<Vector2>();
    };
    _playerControl.Gameplay.LeftStick.canceled += context => _leftStickInput = Vector2.zero;
  }

  private void Stand()
  {
    if (_moveSpeed > 1f)
    {
      _moveSpeed = _moveSpeed / 1.7f;
    }
    else
    {
      _moveSpeed = 1f;
      Player.PlayerState.State = PlayerState.EPlayerState.STAND;
    }
  }

  private void Move()
  {
    if (CanRun())
    {
      SetIsRunning();
    }
    else if (CanWalk())
    {
      SetIsWalking();
    }
  }

  private void Roll()
  {
    Player.PlayerState.State = PlayerState.EPlayerState.ROLLING;
    // anim
    // boost speed
    // bonk hitbox
    Player.PlayerState.State = PlayerState.EPlayerState.STAND;
  }

  private void Rotate()
  {
    Vector3 forward = Player.PlayerCamera.MainCamera.transform.forward;
    Vector3 right = Player.PlayerCamera.MainCamera.transform.right;
    forward.y = 0;
    right.y = 0;
    forward.Normalize();
    right.Normalize();
    Vector3 movementDirection = (forward * _leftStickInput.y + right * _leftStickInput.x).normalized;
    Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
    Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
  }

  private void SetIsRunning()
  {
    _moveSpeed = _runSpeedRatio;
    Player.PlayerState.State = PlayerState.EPlayerState.RUN;
  }

  private void SetIsWalking()
  {
    float highestAnalogInput = Math.Abs(_leftStickInput.y) > Math.Abs(_leftStickInput.x) ? Math.Abs(_leftStickInput.y) : Math.Abs(_leftStickInput.x);
    _moveSpeed = highestAnalogInput * 10;
    Player.PlayerState.State = PlayerState.EPlayerState.WALK;
  }

  private bool CanRun()
  {
    return (_leftStickInput.y > 0.5f || _leftStickInput.y < -0.5f || _leftStickInput.x > 0.5f || _leftStickInput.x < -0.5f);
  }

  private bool CanWalk()
  {
    return ((_leftStickInput.y < 0.5f && _leftStickInput.y > 0.02f) || (_leftStickInput.y > -0.5f && _leftStickInput.y < -0.02f) ||
    (_leftStickInput.x < 0.5f && _leftStickInput.x > 0.02f) || (_leftStickInput.x > -0.5f && _leftStickInput.x < -0.02f));
  }

  private bool CanStand()
  {
    return Array.Exists(FreeActionState, element => element == Player.PlayerState.State) && _leftStickInput.y < 0.02f && _leftStickInput.y > -0.02f && _leftStickInput.x < 0.02f && _leftStickInput.x > -0.02f;
  }

  private bool CanMove()
  {
    return Array.Exists(FreeActionState, element => element == Player.PlayerState.State);
  }

  private bool CanRoll()
  {
    return CanMove() && CanRun() && Input.GetKeyDown(KeyCode.H); // TODO new input system
  }

  private bool CanRotate()
  {
    return Array.Exists(FreeActionState, element => element == Player.PlayerState.State) && (_leftStickInput.y != 0f || _leftStickInput.x != 0f);
  }

  private void MovePlayerByInputAndCamera()
  {
    Vector3 stickDirection = new Vector3(_leftStickInput.x, 0, _leftStickInput.y);

    Vector3 cameraDirection = Player.PlayerCamera.MainCamera.transform.forward;
    cameraDirection.y = 0.0f;
    Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(cameraDirection));

    Vector3 moveDirection = referentialShift * stickDirection;
    if (moveDirection.magnitude > 0.1f)
    {
      Vector3 newPosition = Player.transform.position + (moveDirection * _moveSpeed * Time.deltaTime);
      Player.Rigidbody.MovePosition(newPosition);
    }
  }

  private void ResetSpeed()
  {
    _moveSpeed = _defaultMoveSpeed;
  }

  public void SetupReadMode() 
  {
    Player.PlayerState.State = PlayerState.EPlayerState.STAND;
    // animation stand
    ResetSpeed();
    // camera int
  }

  public void ExitReadMode()
  {
    Player.PlayerState.State = PlayerState.EPlayerState.STAND;
        // animation stand
    ResetSpeed();
    // camera out
  }
}

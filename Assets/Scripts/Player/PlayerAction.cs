using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAction : MonoBehaviour
{
    public Player Player;

    private readonly float _rotationSpeed = 5f;
    private readonly float _runSpeedRatio = 7f;

    [SerializeField]
    private float SpeedRatio = 1f;


    [SerializeField]
    private Vector2 LeftStickInput;

    [SerializeField]
    private Vector2 RightStickInput;

    private PlayerControl PlayerControl;

    [SerializeField]
    private float Angle = 0f;

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
        if (CanStand())
        {
            Stand();
        }
        if (CanMove())
        {
            Move();
        }
        if (CanRoll())
        {
            Roll();
        }
    }

    private void ReadInput()
    {
        this.PlayerControl = new PlayerControl();

        this.PlayerControl.Gameplay.LeftStick.Enable();
        this.PlayerControl.Gameplay.LeftStick.performed += context => { 
            this.LeftStickInput = context.ReadValue<Vector2>();
        };
        this.PlayerControl.Gameplay.LeftStick.canceled += context => this.LeftStickInput = Vector2.zero;
    }

    private void Stand()
    {
        if (this.SpeedRatio > 1f)
        {
            this.SpeedRatio = this.SpeedRatio / 1.7f;
            this.Player.transform.Translate(Vector3.forward * Time.deltaTime * this.LeftStickInput.y * this.SpeedRatio);
            this.Player.transform.Translate(Vector3.right * Time.deltaTime * this.LeftStickInput.x * this.SpeedRatio);
        }
        else
        {
            this.SpeedRatio = 1f;
            this.Player.PlayerState.State = PlayerState.EPlayerState.STAND;
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
        this.Player.transform.Translate(Vector3.forward * Time.deltaTime * this.LeftStickInput.y * this.SpeedRatio);
        this.Player.transform.Translate(Vector3.right * Time.deltaTime * this.LeftStickInput.x * this.SpeedRatio);
    }

    private void Roll()
    {
        this.Player.PlayerState.State = PlayerState.EPlayerState.ROLLING;
        // anim
        // boost speed
        // bonk hitbox
        this.Player.PlayerState.State = PlayerState.EPlayerState.STAND;
    }

    private void Rotate()
    {
        Vector3 forward = this.Player.PlayerCamera.MainCamera.transform.forward;
        Vector3 right = this.Player.PlayerCamera.MainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 movementDirection = (forward * this.LeftStickInput.y + right * this.LeftStickInput.x).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        this.Player.transform.rotation = Quaternion.Slerp(this.Player.transform.rotation, targetRotation, this._rotationSpeed * Time.deltaTime);
    }

    private void SetIsRunning()
    {
        this.SpeedRatio = this._runSpeedRatio;
        this.Player.PlayerState.State = PlayerState.EPlayerState.RUN;
    }

    private void SetIsWalking()
    {
        float highestAnalogInput = Math.Abs(this.LeftStickInput.y) > Math.Abs(this.LeftStickInput.x) ? Math.Abs(this.LeftStickInput.y) : Math.Abs(this.LeftStickInput.x);
        this.SpeedRatio = highestAnalogInput * 10;
        this.Player.PlayerState.State = PlayerState.EPlayerState.WALK;
    }

    private bool CanRun()
    {
        return (this.LeftStickInput.y > 0.5f || this.LeftStickInput.y < -0.5f || this.LeftStickInput.x > 0.5f || this.LeftStickInput.x < -0.5f);
    }

    private bool CanWalk()
    {
        return ((this.LeftStickInput.y < 0.5f && this.LeftStickInput.y > 0.02f) || (this.LeftStickInput.y > -0.5f && this.LeftStickInput.y < -0.02f) ||
        (this.LeftStickInput.x < 0.5f && this.LeftStickInput.x > 0.02f) || (this.LeftStickInput.x > -0.5f && this.LeftStickInput.x < -0.02f));
    }

    private bool CanStand()
    {
        return Array.Exists(this.FreeActionState, element => element == this.Player.PlayerState.State) && this.LeftStickInput.y < 0.02f && this.LeftStickInput.y > -0.02f && this.LeftStickInput.x < 0.02f && this.LeftStickInput.x > -0.02f;
    }

    private bool CanMove()
    {
        return Array.Exists(this.FreeActionState, element => element == this.Player.PlayerState.State);
    }

    private bool CanRoll()
    {
        return this.CanMove() && this.CanRun() && Input.GetKeyDown(KeyCode.H); // TODO new input system
    }

    private bool CanRotate()
    {
        return Array.Exists(this.FreeActionState, element => element == this.Player.PlayerState.State) && (this.LeftStickInput.y != 0f || this.LeftStickInput.x != 0f);
    }
}

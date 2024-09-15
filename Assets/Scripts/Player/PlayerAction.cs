using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public Player Player;
    private readonly float RunSpeedRatio = 7f;
    private float SpeedRatio = 1f;
    private float HorizontalInput = 0f;
    private float VerticalInput = 0f;

    void Start()
    {

    }

    void Update()
    {
        ReadInput();
        Move();
        Roll();
    }

    private void ReadInput()
    {
        this.HorizontalInput = Input.GetAxis("Horizontal");
        this.VerticalInput = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        if (IsWalking())
        {
            SetIsWalking();
        }
        else if (IsRunning())
        {
            SetIsRunning();
        }
        this.Player.transform.Translate(Vector3.forward * Time.deltaTime * this.VerticalInput * this.SpeedRatio);
        this.Player.transform.Translate(Vector3.right * Time.deltaTime * this.HorizontalInput * this.SpeedRatio);
    }

    private void Roll()
    {

    }

    private void SetIsWalking()
    {
        this.SpeedRatio = this.WalkSpeedRatio; // dynamic instead ? depending on control stick
        this.Player.PlayerState.State = PlayerState.EPlayerState.WALK;
    }

    private void SetIsRunning()
    {
        this.SpeedRatio = this.RunSpeedRatio;
        this.Player.PlayerState.State = PlayerState.EPlayerState.RUN;

    }

    private void SetIsStanding()
    {
        this.Player.PlayerState.State = PlayerState.EPlayerState.STAND;
    }

    private bool IsWalking()
    {
        return this.VerticalInput < 0.6 || this.HorizontalInput < 0.6; // TODO between -1 and 1, with 0 for neutral
    }

    private bool IsRunning()
    {
        return this.VerticalInput < 500f || this.HorizontalInput < 500f;  // TODO
    }

    private bool IsStanding()
    {
        return this.VerticalInput < 100f && this.HorizontalInput < 100f;  // TODO
    }
}

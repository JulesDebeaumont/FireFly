using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/jm991/UnityThirdPersonTutorial/blob/master/Assets/Scripts/ThirdPersonCamera.cs#L303


public class PlayerCamera : MonoBehaviour
{
    private readonly float _firstPersonCameraSpeed = 0.4f;
    private readonly Vector2 _firstPersonXAxisClamp = new Vector2(-70f, 90f);

    public Player Player;
    public Camera MainCamera;

    [SerializeField]
    private ECameraState CameraState = ECameraState.STANDARD;

    private Vector3 PlayerOffset;

    [SerializeField]
    private Vector3 LookAt;

    private Vector3 TargetPosition;
    private Vector3 SavedRigToGoal;
    private Vector3 VelocityCamSmooth = Vector3.zero;
    private float CamSmoothDampTime = 0.1f;
    private CameraPosition FirstPersonCameraPosition;
    private PlayerControl PlayerControl;


    [SerializeField]
    private float xAxisRot = 0.0f;

    [SerializeField]
    private float lookWeight = 0f;

    [SerializeField]
    private float DistanceUp;

    [SerializeField]
    private bool HasStartedFocus = false;

    [SerializeField]
    private bool HasStartedUnfocus = false;

    [SerializeField]
    private bool HasBlackBar = false;

    [SerializeField]
    private bool ZPress = false;

    [SerializeField]
    private Vector2 LeftStickInput = Vector2.zero;

    [SerializeField]
    private Vector2 RightStickInput = Vector2.zero;

    void Awake()
    {
        ReadInput();
    }

    // Start is called before the first frame update
    void Start() // put in awake ?
    {
        this.FirstPersonCameraPosition = new CameraPosition();
        this.FirstPersonCameraPosition.Init
            (
                new Vector3(0f, 1.6f, 0f),
                new GameObject().transform,
                this.Player.transform
            );
        this.PlayerOffset = this.Player.transform.position + new Vector3(0f, this.DistanceUp, 0f);
        this.SavedRigToGoal = RigToGoalDirection();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetupVectors();
        SetupCameraMode();
        ActionByCameraMode();
    }

    private void ReadInput()
    {
        this.PlayerControl = new PlayerControl();

        this.PlayerControl.Gameplay.LeftStick.Enable();
        this.PlayerControl.Gameplay.LeftStick.performed += context => this.LeftStickInput = context.ReadValue<Vector2>();
        this.PlayerControl.Gameplay.LeftStick.canceled += context => this.LeftStickInput = Vector2.zero;

        this.PlayerControl.Gameplay.RightStick.Enable();
        this.PlayerControl.Gameplay.RightStick.performed += context => this.RightStickInput = context.ReadValue<Vector2>();
        this.PlayerControl.Gameplay.RightStick.canceled += context => this.RightStickInput = Vector2.zero;

        this.PlayerControl.Gameplay.Z.Enable();
        this.PlayerControl.Gameplay.Z.performed += context => this.ZPress = true;
        this.PlayerControl.Gameplay.Z.canceled += context => this.ZPress = false;
    }

    private void SetupVectors()
    {
        this.PlayerOffset = this.Player.transform.position + (this.DistanceUp * this.Player.transform.up);
        this.LookAt = this.PlayerOffset;
        this.TargetPosition = Vector3.zero;
    }

    private void SetupCameraMode()
    {
        if (this.ZPress == true)
        {
            this.CameraState = ECameraState.FOCUS;
            if (this.HasBlackBar == false && this.HasStartedFocus == false)
            {
                EnableFocusMode();
            }
            return;
        }
        if (this.HasBlackBar == true && this.HasStartedUnfocus == false)
        {
            DisableFocusMode();
        }
        if (CPositionEligibleForFirstPersonMode() && this.CameraState != ECameraState.FIRST_PERSON &&
            this.CameraState != ECameraState.FREE && this.Player.PlayerState.State == PlayerState.EPlayerState.STAND)
        {
            this.CameraState = ECameraState.FIRST_PERSON;
            this.Player.PlayerState.State = PlayerState.EPlayerState.LOOKING;
            this.xAxisRot = 0f;
            this.lookWeight = 0f;
            return;
        }
        if (CPositionEligibleForFreeMode() && this.CameraState != ECameraState.FREE && this.CameraState != ECameraState.FIRST_PERSON)
        {
            this.CameraState = ECameraState.FREE;
            this.SavedRigToGoal = Vector3.zero;
            return;
        }
    }

    private void ActionByCameraMode()
    {
        switch (this.CameraState)
        {
            case ECameraState.FOCUS:
                //RunFocusMode();
                break;

            case ECameraState.FREE:
                //RunFreeMode();
                break;

            case ECameraState.FIRST_PERSON:
                RunFirstPersonMode();
                break;

            case ECameraState.STANDARD:
                //RunStandardMode();
                break;
        }
        //CompensateForWalls();
        SmoothPosition(this.MainCamera.transform.position, this.TargetPosition);
        this.MainCamera.transform.LookAt(this.LookAt);
    }

    private void SmoothPosition(Vector3 fromPosition, Vector3 toPosition)
    {
        this.MainCamera.transform.position = Vector3.SmoothDamp(fromPosition, toPosition, ref this.VelocityCamSmooth, this.CamSmoothDampTime);
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {

    }

    private void RunFirstPersonMode()
    {
        this.xAxisRot += (this.LeftStickInput.y * 0.5f * this._firstPersonCameraSpeed);
        this.xAxisRot = Mathf.Clamp(this.xAxisRot, this._firstPersonXAxisClamp.x, this._firstPersonXAxisClamp.y);
        this.FirstPersonCameraPosition.Transform.localRotation = Quaternion.Euler(this.xAxisRot, 0f, 0f);

        Quaternion rotationShift = Quaternion.FromToRotation(this.MainCamera.transform.forward, this.FirstPersonCameraPosition.Transform.forward);
        this.MainCamera.transform.rotation = rotationShift * this.MainCamera.transform.rotation;

        this.lookWeight = Mathf.Lerp(this.lookWeight, 1.0f, Time.deltaTime * this._firstPersonCameraSpeed);

        Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, 120f * (this.LeftStickInput.x < 0f ? -1f : 1f), 0f), Mathf.Abs(this.LeftStickInput.x));
        Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
        this.Player.transform.rotation = (this.Player.transform.rotation * deltaRotation);

        this.TargetPosition = this.FirstPersonCameraPosition.Transform.position;

        this.LookAt = Vector3.Lerp(this.TargetPosition + this.Player.transform.forward, this.MainCamera.transform.position + this.MainCamera.transform.forward, 0.1f * Time.deltaTime);
        this.LookAt = (Vector3.Lerp(this.MainCamera.transform.position + this.MainCamera.transform.forward, this.LookAt, Vector3.Distance(this.MainCamera.transform.position, this.FirstPersonCameraPosition.Transform.position)));
    }

    public void FadeToBlack()
    {
        // https://discussions.unity.com/t/free-basic-camera-fade-in-script/686081
    }

    public void FadeFromBlack()
    {

    }

    public void AllBlack()
    {

    }

    public void EnableFocusMode()
    {
        this.HasStartedFocus = true;
        // appear black bar
        this.HasBlackBar = true;
        this.HasStartedFocus = false;
    }

    public void DisableFocusMode()
    {
        this.HasStartedUnfocus = true;
        // remove black bar
        this.HasBlackBar = false;
        this.HasStartedUnfocus = false;
    }

    private Vector3 RigToGoalDirection()
    {
        Vector3 rigToGoalDirection = Vector3.Normalize(this.PlayerOffset - this.MainCamera.transform.position);
        rigToGoalDirection.y = 0f;
        return rigToGoalDirection;
    }

    private void ResetCamera()
    {
        this.lookWeight = Mathf.Lerp(this.lookWeight, 0f, Time.deltaTime * this._firstPersonCameraSpeed);
        this.MainCamera.transform.localRotation = Quaternion.Lerp(this.MainCamera.transform.localRotation, Quaternion.identity, Time.deltaTime);
    }

    private bool CPositionEligibleForFirstPersonMode()
    {
        return this.RightStickInput.x < 0.1f && this.RightStickInput.x > -0.1f && this.RightStickInput.y > 0.3f;
    }

    private bool CPositionEligibleForFreeMode()
    {
        return this.RightStickInput.x > 0.1f || this.RightStickInput.x < -0.1f || (this.RightStickInput.y > 0.1f && this.RightStickInput.y < 0.4f) || this.RightStickInput.y < -0.1f;
    }

    private enum ECameraState
    {
        STANDARD,
        FOCUS,
        FREE,
        FIRST_PERSON
    }

}

struct CameraPosition
{
    public Vector3 Position { get; set; }
    public Transform Transform { get; set; }

    public void Init(Vector3 position, Transform transform, Transform parent)
    {
        Position = position;
        Transform = transform;
        Transform.localPosition = Vector3.zero;
        Transform.localPosition = position;
        Transform.parent = parent;
    }
}

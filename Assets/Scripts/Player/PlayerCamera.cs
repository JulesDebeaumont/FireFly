using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/jm991/UnityThirdPersonTutorial/blob/master/Assets/Scripts/ThirdPersonCamera.cs#L303


public class PlayerCamera : MonoBehaviour
{
  public Player Player;
  public Camera MainCamera;

  private readonly float _firstPersonCameraSpeed = 0.4f;
  private readonly Vector2 _firstPersonXAxisClamp = new Vector2(-70f, 90f);
  private readonly float _camSmoothDampTime = 0.1f;

  private Vector3 _targetPosition;
  private Vector3 _savedRigToGoal;
  private Vector3 _velocityCamSmooth = Vector3.zero;
  private CameraPosition _firstPersonCameraPosition;
  private PlayerControl _playerControl;
  private Vector3 _playerOffset;


  [SerializeField]
  private ECameraState _cameraState = ECameraState.STANDARD;

  [SerializeField]
  private Vector3 _lookAt;

  [SerializeField]
  private float _xAxisRot = 0.0f;

  [SerializeField]
  private float _lookWeight = 0f;

  [SerializeField]
  private readonly float _distanceUp;

  [SerializeField]
  private bool _hasStartedFocus = false;

  [SerializeField]
  private bool _hasStartedUnfocus = false;

  [SerializeField]
  private bool _hasBlackBar = false;

  [SerializeField]
  private bool _zPress = false;

  [SerializeField]
  private Vector2 _leftStickInput = Vector2.zero;

  [SerializeField]
  private Vector2 _rightStickInput = Vector2.zero;

  void Awake()
  {
    ReadInput();
  }

  // Start is called before the first frame update
  void Start() // put in awake ?
  {
    _firstPersonCameraPosition = new CameraPosition();
    _firstPersonCameraPosition.Init
        (
            "First person camera",
            new Vector3(0f, 1.6f, 0f),
            new GameObject().transform,
            Player.transform
        );
    _playerOffset = Player.transform.position + new Vector3(0f, _distanceUp, 0f);
    _savedRigToGoal = RigToGoalDirection();
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
    _playerControl = new PlayerControl();

    _playerControl.Gameplay.LeftStick.Enable();
    _playerControl.Gameplay.LeftStick.performed += context => _leftStickInput = context.ReadValue<Vector2>();
    _playerControl.Gameplay.LeftStick.canceled += context => _leftStickInput = Vector2.zero;

    _playerControl.Gameplay.RightStick.Enable();
    _playerControl.Gameplay.RightStick.performed += context => _rightStickInput = context.ReadValue<Vector2>();
    _playerControl.Gameplay.RightStick.canceled += context => _rightStickInput = Vector2.zero;

    _playerControl.Gameplay.Z.Enable();
    _playerControl.Gameplay.Z.performed += context => _zPress = true;
    _playerControl.Gameplay.Z.canceled += context => _zPress = false;
  }

  private void SetupVectors()
  {
    _playerOffset = Player.transform.position + (_distanceUp * Player.transform.up);
    _lookAt = _playerOffset;
    _targetPosition = Vector3.zero;
  }

  private void SetupCameraMode()
  {
    if (_zPress == true)
    {
      _cameraState = ECameraState.FOCUS;
      if (_hasBlackBar == false && _hasStartedFocus == false)
      {
        EnableFocusMode();
      }
      return;
    }
    if (_hasBlackBar == true && _hasStartedUnfocus == false)
    {
      DisableFocusMode();
    }
    if (CPositionEligibleForFirstPersonMode() && _cameraState != ECameraState.FIRST_PERSON &&
        _cameraState != ECameraState.FREE && Player.PlayerState.State == PlayerState.EPlayerState.STAND)
    {
      _cameraState = ECameraState.FIRST_PERSON;
      Player.PlayerState.State = PlayerState.EPlayerState.LOOKING;
      _xAxisRot = 0f;
      _lookWeight = 0f;
      return;
    }
    if (CPositionEligibleForFreeMode() && _cameraState != ECameraState.FREE && _cameraState != ECameraState.FIRST_PERSON)
    {
      _cameraState = ECameraState.FREE;
      _savedRigToGoal = Vector3.zero;
      return;
    }
  }

  private void ActionByCameraMode()
  {
    switch (_cameraState)
    {
      case ECameraState.FOCUS:
        //RunFocusMode();
        break;

      case ECameraState.FREE:
        RunFreeMode();
        break;

      case ECameraState.FIRST_PERSON:
        RunFirstPersonMode();
        break;

      case ECameraState.STANDARD:
        //RunStandardMode();
        break;
    }
    //CompensateForWalls();
    SmoothPosition(MainCamera.transform.position, _targetPosition);
    MainCamera.transform.LookAt(_lookAt);
  }

  private void SmoothPosition(Vector3 fromPosition, Vector3 toPosition)
  {
    MainCamera.transform.position = Vector3.SmoothDamp(fromPosition, toPosition, ref _velocityCamSmooth, _camSmoothDampTime);
  }

  private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
  {

  }

  private void RunFirstPersonMode()
  {
    _xAxisRot += _leftStickInput.y * 0.5f * _firstPersonCameraSpeed;
    _xAxisRot = Mathf.Clamp(_xAxisRot, _firstPersonXAxisClamp.x, _firstPersonXAxisClamp.y);
    _firstPersonCameraPosition.Transform.localRotation = Quaternion.Euler(_xAxisRot, 0f, 0f);

    Quaternion rotationShift = Quaternion.FromToRotation(MainCamera.transform.forward, _firstPersonCameraPosition.Transform.forward);
    MainCamera.transform.rotation = rotationShift * MainCamera.transform.rotation;

    _lookWeight = Mathf.Lerp(_lookWeight, 1.0f, Time.deltaTime * _firstPersonCameraSpeed);

    Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, 120f * (_leftStickInput.x < 0f ? -1f : 1f), 0f), Mathf.Abs(_leftStickInput.x));
    Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
    Player.transform.rotation = Player.transform.rotation * deltaRotation;

    _targetPosition = _firstPersonCameraPosition.Transform.position;

    _lookAt = Vector3.Lerp(_targetPosition + Player.transform.forward, MainCamera.transform.position + MainCamera.transform.forward, 0.1f * Time.deltaTime);
    _lookAt = Vector3.Lerp(MainCamera.transform.position + MainCamera.transform.forward, _lookAt, Vector3.Distance(MainCamera.transform.position, _firstPersonCameraPosition.Transform.position));
  }

  private void RunFreeMode()
  {/*
    this.lookWeight = Mathf.Lerp(this.lookWeight, 0.0f, Time.deltaTime * firstPersonLookSpeed);

    Vector3 rigToGoal = characterOffset - cameraXform.position;
    rigToGoal.y = 0f;
    Debug.DrawRay(cameraXform.transform.position, rigToGoal, Color.red);

    // Panning in and out
    // If statement works for positive values; don't tween if stick not increasing in either direction; also don't tween if user is rotating
    // Checked against rightStickThreshold because very small values for rightY mess up the Lerp function
    if (rightY < lastStickMin && rightY < -1f * rightStickThreshold && rightY <= rightStickPrevFrame.y && Mathf.Abs(rightX) < rightStickThreshold)
    {
      // Zooming out
      distanceUpFree = Mathf.Lerp(distanceUp, distanceUp * distanceUpMultiplier, Mathf.Abs(rightY));
      distanceAwayFree = Mathf.Lerp(distanceAway, distanceAway * distanceAwayMultipler, Mathf.Abs(rightY));
      targetPosition = characterOffset + followXform.up * distanceUpFree - RigToGoalDirection * distanceAwayFree;
      lastStickMin = rightY;
    }
    else if (rightY > rightStickThreshold && rightY >= rightStickPrevFrame.y && Mathf.Abs(rightX) < rightStickThreshold)
    {
      // Zooming in
      // Subtract height of camera from height of player to find Y distance
      distanceUpFree = Mathf.Lerp(Mathf.Abs(transform.position.y - characterOffset.y), camMinDistFromChar.y, rightY);
      // Use magnitude function to find X distance	
      distanceAwayFree = Mathf.Lerp(rigToGoal.magnitude, camMinDistFromChar.x, rightY);
      targetPosition = characterOffset + followXform.up * distanceUpFree - RigToGoalDirection * distanceAwayFree;
      lastStickMin = float.PositiveInfinity;
    }

    // Store direction only if right stick inactive
    if (rightX != 0 || rightY != 0)
    {
      savedRigToGoal = RigToGoalDirection;
    }


    // Rotating around character
    cameraXform.RotateAround(characterOffset, followXform.up, freeRotationDegreePerSecond * (Mathf.Abs(rightX) > rightStickThreshold ? rightX : 0f));

    // Still need to track camera behind player even if they aren't using the right stick; achieve this by saving distanceAwayFree every frame
    if (targetPosition == Vector3.zero)
    {
      targetPosition = characterOffset + followXform.up * distanceUpFree - savedRigToGoal * distanceAwayFree;
    }*/
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
    _hasStartedFocus = true;
    // appear black bar
    _hasBlackBar = true;
    _hasStartedFocus = false;
  }

  public void DisableFocusMode()
  {
    _hasStartedUnfocus = true;
    // remove black bar
    _hasBlackBar = false;
    _hasStartedUnfocus = false;
  }

  private Vector3 RigToGoalDirection()
  {
    Vector3 rigToGoalDirection = Vector3.Normalize(_playerOffset - MainCamera.transform.position);
    rigToGoalDirection.y = 0f;
    return rigToGoalDirection;
  }

  private void ResetCamera()
  {
    _lookWeight = Mathf.Lerp(_lookWeight, 0f, Time.deltaTime * _firstPersonCameraSpeed);
    MainCamera.transform.localRotation = Quaternion.Lerp(MainCamera.transform.localRotation, Quaternion.identity, Time.deltaTime);
  }

  private bool CPositionEligibleForFirstPersonMode()
  {
    return _rightStickInput.x < 0.1f && _rightStickInput.x > -0.1f && _rightStickInput.y > 0.3f;
  }

  private bool CPositionEligibleForFreeMode()
  {
    return _rightStickInput.x > 0.1f || _rightStickInput.x < -0.1f || (_rightStickInput.y > 0.1f && _rightStickInput.y < 0.4f) || _rightStickInput.y < -0.1f;
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

  public void Init(string gameObjectName, Vector3 position, Transform transform, Transform parent)
  {
    Position = position;
    Transform = transform;
    Transform.name = gameObjectName;
    Transform.localPosition = Vector3.zero;
    Transform.localPosition = position;
    Transform.parent = parent;
  }
}

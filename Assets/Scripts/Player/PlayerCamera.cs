using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
  public Player Player;
  public Camera MainCamera;

  private readonly float _firstPersonCameraSpeed = 0.4f;
  private readonly Vector2 _firstPersonXAxisClamp = new Vector2(-70f, 90f);
  private readonly float _camSmoothDampTime = 0.1f;
  private readonly float _rightStickThreshold = 0.3f;
  private readonly float _distanceUp = 1.15f;
  private readonly float _distanceUpMultiplier = 2.5f;
  private readonly float _distanceAway = 3.5f;
  private readonly float _distanceAwayMultiplier = 2f;
  private readonly Vector2 _camMinDistanceFromPlayer = new Vector2(1f, -0.5f);
  private readonly float _freeRotationDegreePerSecond = -0.2f;
	private readonly float _lookDirDampTime = 0.1f;

  private Vector3 _targetPosition;
  private Vector3 _velocityCamSmooth = Vector3.zero;
  private CameraPosition _firstPersonCameraPosition;
  private PlayerControl _playerControl;
  private Vector3 _playerOffset;
  private Vector2 _rightStickPrevFrame = Vector2.zero;
  private Vector3 _velocityLookDir = Vector3.zero;

  [SerializeField] private ECameraState _cameraState = ECameraState.STANDARD;

  [SerializeField] private Vector3 _lookAt;

  [SerializeField] private float _xAxisRot = 0.0f;

  [SerializeField] private Vector3 _lookDirection;

  [SerializeField] private Vector3 _currentLookDirection;

  [SerializeField] private bool _hasStartedFocus = false;

  [SerializeField] private bool _hasStartedUnfocus = false;

  [SerializeField] private bool _hasBlackBar = false;

  [SerializeField] private bool _zPress = false;

  [SerializeField] private float _distanceUpFree;

  [SerializeField] private float _distanceAwayFree;

  [SerializeField] private Vector3 _savedRigToGoal;

  [SerializeField] private float _lastStickMin = float.PositiveInfinity;

  [SerializeField] private Vector2 _leftStickInput = Vector2.zero;

  [SerializeField] private Vector2 _rightStickInput = Vector2.zero;

  void Awake()
  {
    ReadInput();
  }

  // Start is called before the first frame update
  void Start()
  {
    _lookDirection = Player.transform.forward;
    _currentLookDirection = Player.transform.forward;
    _firstPersonCameraPosition = new CameraPosition();
    _firstPersonCameraPosition.Init
        (
            "First person camera",
            new Vector3(0f, 1.6f, 0f),
            new GameObject().transform,
            Player.transform
        );
    _playerOffset = Player.transform.position + new Vector3(0f, _distanceUp, 0f);
    _distanceUpFree = _distanceUp;
    _distanceAwayFree = _distanceAway;
    _savedRigToGoal = RigToGoalDirection();
  }

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
        _cameraState != ECameraState.FREE && Player.PlayerState.GetPlayerState() == PlayerState.EPlayerState.STAND)
    {
      _cameraState = ECameraState.FIRST_PERSON;
      Player.PlayerState.GetPlayerState() = PlayerState.EPlayerState.LOOKING;
      _xAxisRot = 0f;
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
        RunFocusMode();
        break;

      case ECameraState.FREE:
        RunFreeMode();
        break;

      case ECameraState.FIRST_PERSON:
        RunFirstPersonMode();
        break;

      case ECameraState.STANDARD:
        RunStandardMode();
        break;
    }
    //CompensateForWalls();
    SmoothPosition(MainCamera.transform.position, _targetPosition);
    MainCamera.transform.LookAt(_lookAt);
  }

  private void SmoothPosition(Vector3 fromPosition, Vector3 toPosition)
  {
    MainCamera.transform.position = Vector3.SmoothDamp(fromPosition, toPosition, ref _velocityCamSmooth, _camSmoothDampTime * Time.deltaTime);
  }

  private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
  {
		RaycastHit wallHit = new RaycastHit();		
		if (Physics.Linecast(fromObject, toTarget, out wallHit)) 
		{
			toTarget = wallHit.point;
		}		
		/*
		Vector3 camPosCache = GetComponent<Camera>().transform.position;
		GetComponent<Camera>().transform.position = toTarget;
		viewFrustum = DebugDraw.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);
		
		for (int i = 0; i < (viewFrustum.Length / 2); i++)
		{
			RaycastHit cWHit = new RaycastHit();
			RaycastHit cCWHit = new RaycastHit();
			
			// Cast lines in both directions around near clipping plane bounds
			while (Physics.Linecast(viewFrustum[i], viewFrustum[(i + 1) % (viewFrustum.Length / 2)], out cWHit) ||
			       Physics.Linecast(viewFrustum[(i + 1) % (viewFrustum.Length / 2)], viewFrustum[i], out cCWHit))
			{
				Vector3 normal = wallHit.normal;
				if (wallHit.normal == Vector3.zero)
				{
					// If there's no available wallHit, use normal of geometry intersected by LineCasts instead
					if (cWHit.normal == Vector3.zero)
					{
						if (cCWHit.normal == Vector3.zero)
						{
							Debug.LogError("No available geometry normal from near clip plane LineCasts. Something must be amuck.", this);
						}
						else
						{
							normal = cCWHit.normal;
						}
					}	
					else
					{
						normal = cWHit.normal;
					}
				}
				
				toTarget += (compensationOffset * normal);
				GetComponent<Camera>().transform.position += toTarget;
				
				// Recalculate positions of near clip plane
				viewFrustum = DebugDraw.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);
			}
		}
		
		GetComponent<Camera>().transform.position = camPosCache;
		viewFrustum = DebugDraw.CalculateViewFrustum(GetComponent<Camera>(), ref nearClipDimensions);*/
  }

  private void RunFirstPersonMode()
  {
    _xAxisRot += _leftStickInput.y * 0.5f * _firstPersonCameraSpeed;
    _xAxisRot = Mathf.Clamp(_xAxisRot, _firstPersonXAxisClamp.x, _firstPersonXAxisClamp.y);
    _firstPersonCameraPosition.Transform.localRotation = Quaternion.Euler(_xAxisRot, 0f, 0f);

    Quaternion rotationShift = Quaternion.FromToRotation(MainCamera.transform.forward, _firstPersonCameraPosition.Transform.forward);
    MainCamera.transform.rotation = rotationShift * MainCamera.transform.rotation;

    Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, 120f * (_leftStickInput.x < 0f ? -1f : 1f), 0f), Mathf.Abs(_leftStickInput.x));
    Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
    Player.transform.rotation = Player.transform.rotation * deltaRotation;

    _targetPosition = _firstPersonCameraPosition.Transform.position;

    _lookAt = Vector3.Lerp(_targetPosition + Player.transform.forward, MainCamera.transform.position + MainCamera.transform.forward, 0.1f * Time.deltaTime);
    _lookAt = Vector3.Lerp(MainCamera.transform.position + MainCamera.transform.forward, _lookAt, Vector3.Distance(MainCamera.transform.position, _firstPersonCameraPosition.Transform.position));
  }

  private void RunFreeMode()
  {
    Vector3 rigToGoal = _playerOffset - MainCamera.transform.position;
    rigToGoal.y = 0f;

    if (_rightStickInput.y < float.PositiveInfinity && _rightStickInput.y < -1f * _rightStickThreshold && _rightStickInput.y <= _rightStickPrevFrame.y && Mathf.Abs(_rightStickInput.x) < _rightStickThreshold)
    {
      _distanceUpFree = Mathf.Lerp(_distanceUp, _distanceUp * _distanceUpMultiplier, Mathf.Abs(_rightStickInput.y));
      _distanceAwayFree = Mathf.Lerp(_distanceAway, _distanceAway * _distanceAwayMultiplier, Mathf.Abs(_rightStickInput.y));
      _targetPosition = _playerOffset + Player.transform.up * _distanceUpFree - RigToGoalDirection() * _distanceAwayFree;
      _lastStickMin = _rightStickInput.y;
    }
    else if (_rightStickInput.y > _rightStickThreshold && _rightStickInput.y >= _rightStickPrevFrame.y && Mathf.Abs(_rightStickInput.x) < _rightStickThreshold)
    {
      _distanceUpFree = Mathf.Lerp(Mathf.Abs(MainCamera.transform.position.y - _playerOffset.y), _camMinDistanceFromPlayer.y, _rightStickInput.y);
      _distanceAwayFree = Mathf.Lerp(rigToGoal.magnitude, _camMinDistanceFromPlayer.x, _rightStickInput.y);
      _targetPosition = _playerOffset + Player.transform.up * _distanceUpFree - RigToGoalDirection() * _distanceAwayFree;
      _lastStickMin = float.PositiveInfinity;
    }

    if (_rightStickInput.x != 0 || _rightStickInput.y != 0)
    {
      _savedRigToGoal = RigToGoalDirection();
    }

    MainCamera.transform.RotateAround(_playerOffset, Player.transform.up, _freeRotationDegreePerSecond * (Mathf.Abs(_rightStickInput.x) > _rightStickThreshold ? _rightStickInput.x : 0f));

    if (_targetPosition == Vector3.zero)
    {
      _targetPosition = _playerOffset + Player.transform.up * _distanceUpFree - _savedRigToGoal * _distanceAwayFree;
    }
  }

  private void RunStandardMode()
  {
    ResetCamera();
    _lookDirection = Vector3.Lerp(Player.transform.right * (_leftStickInput.x < 0 ? 1f : -1f), Player.transform.forward * (_leftStickInput.y < 0 ? -1f : 1f), Mathf.Abs(Vector3.Dot(MainCamera.transform.forward, Player.transform.forward)));
    _currentLookDirection = Vector3.Normalize(_playerOffset - MainCamera.transform.position);
    _currentLookDirection.y = 0;
    _currentLookDirection = Vector3.SmoothDamp(_currentLookDirection, _lookDirection, ref _velocityLookDir, _lookDirDampTime);
    _targetPosition = _playerOffset + Player.transform.up * _distanceUp - Vector3.Normalize(_currentLookDirection) * _distanceAway;
  }

  private void RunFocusMode()
  {
    ResetCamera();
    _lookDirection = Player.transform.forward;
    _currentLookDirection = Player.transform.forward;
    _targetPosition = _playerOffset + Player.transform.up * _distanceUp - _lookDirection * _distanceAway;
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

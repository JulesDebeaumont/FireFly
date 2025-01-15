using System;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public Player player;
        public Camera mainCamera;

        [SerializeField] private ECameraState cameraState = ECameraState.STANDARD;
        [SerializeField] private Vector3 lookAt;
        [SerializeField] private float xAxisRot;
        [SerializeField] private Vector3 lookDirection;
        [SerializeField] private Vector3 currentLookDirection;
        [SerializeField] private bool hasStartedFocus;
        [SerializeField] private bool hasStartedUnfocus;
        [SerializeField] private bool hasBlackBar;
        [SerializeField] private bool zPress;
        [SerializeField] private float distanceUpFree;
        [SerializeField] private float distanceAwayFree;
        [SerializeField] private Vector3 savedRigToGoal;
        [SerializeField] private float lastStickMin = float.PositiveInfinity;
        [SerializeField] private Vector2 leftStickInput = Vector2.zero;
        [SerializeField] private Vector2 rightStickInput = Vector2.zero;
        private readonly Vector2 _camMinDistanceFromPlayer = new(1f, -0.5f);
        private const float CamSmoothDampTime = 0.1f;
        private const float DistanceAway = 3.5f;
        private const float DistanceAwayMultiplier = 2f;
        private const float DistanceUp = 1.15f;
        private const float DistanceUpMultiplier = 2.5f;

        private const float FirstPersonCameraSpeed = 0.4f;
        private readonly Vector2 _firstPersonXAxisClamp = new(-70f, 90f);
        private const float FreeRotationDegreePerSecond = -0.2f;
        private readonly float _lookDirDampTime = 0.1f;
        private readonly float _rightStickThreshold = 0.3f;
        private CameraPosition _firstPersonCameraPosition;
        private PlayerControl _playerControl;
        private Vector3 _playerOffset;
        private readonly Vector2 _rightStickPrevFrame = Vector2.zero;

        private Vector3 _targetPosition;
        private Vector3 _velocityCamSmooth = Vector3.zero;
        private Vector3 _velocityLookDir = Vector3.zero;

        private void Awake()
        {
            ReadInput();
        }

        // Start is called before the first frame update
        private void Start()
        {
            lookDirection = player.transform.forward;
            currentLookDirection = player.transform.forward;
            _firstPersonCameraPosition = new CameraPosition();
            _firstPersonCameraPosition.Init
            (
                "First person camera",
                new Vector3(0f, 1.6f, 0f),
                new GameObject().transform,
                player.transform
            );
            _playerOffset = player.transform.position + new Vector3(0f, DistanceUp, 0f);
            distanceUpFree = DistanceUp;
            distanceAwayFree = DistanceAway;
            savedRigToGoal = RigToGoalDirection();
        }

        private void LateUpdate()
        {
            SetupVectors();
            SetupCameraMode();
            ActionByCameraMode();
        }

        private void ReadInput()
        {
            _playerControl = new PlayerControl();

            _playerControl.Gameplay.LeftStick.Enable();
            _playerControl.Gameplay.LeftStick.performed += context => leftStickInput = context.ReadValue<Vector2>();
            _playerControl.Gameplay.LeftStick.canceled += context => leftStickInput = Vector2.zero;

            _playerControl.Gameplay.RightStick.Enable();
            _playerControl.Gameplay.RightStick.performed += context => rightStickInput = context.ReadValue<Vector2>();
            _playerControl.Gameplay.RightStick.canceled += context => rightStickInput = Vector2.zero;

            _playerControl.Gameplay.Z.Enable();
            _playerControl.Gameplay.Z.performed += context => zPress = true;
            _playerControl.Gameplay.Z.canceled += context => zPress = false;
        }

        private void SetupVectors()
        {
            _playerOffset = player.transform.position + DistanceUp * player.transform.up;
            lookAt = _playerOffset;
            _targetPosition = Vector3.zero;
        }

        private void SetupCameraMode()
        {
            if (zPress)
            {
                cameraState = ECameraState.FOCUS;
                if (hasBlackBar == false && hasStartedFocus == false) EnableFocusMode();
                return;
            }

            if (hasBlackBar && hasStartedUnfocus == false) DisableFocusMode();
            if (CPositionEligibleForFirstPersonMode() && cameraState != ECameraState.FIRST_PERSON &&
                cameraState != ECameraState.FREE &&
                player.playerState.GetPlayerState() == PlayerState.EPlayerState.STAND)
            {
                cameraState = ECameraState.FIRST_PERSON;
                player.playerState.SetPlayerState(PlayerState.EPlayerState.LOOKING);
                xAxisRot = 0f;
                return;
            }

            if (CPositionEligibleForFreeMode() && cameraState != ECameraState.FREE &&
                cameraState != ECameraState.FIRST_PERSON)
            {
                cameraState = ECameraState.FREE;
                savedRigToGoal = Vector3.zero;
            }
        }

        private void ActionByCameraMode()
        {
            switch (cameraState)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //CompensateForWalls();
            SmoothPosition(mainCamera.transform.position, _targetPosition);
            mainCamera.transform.LookAt(lookAt);
        }

        private void SmoothPosition(Vector3 fromPosition, Vector3 toPosition)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(fromPosition, toPosition, ref _velocityCamSmooth,
                CamSmoothDampTime * Time.deltaTime);
        }

        private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
        {
            var wallHit = new RaycastHit();
            if (Physics.Linecast(fromObject, toTarget, out wallHit)) toTarget = wallHit.point;
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
            xAxisRot += leftStickInput.y * 0.5f * FirstPersonCameraSpeed;
            xAxisRot = Mathf.Clamp(xAxisRot, _firstPersonXAxisClamp.x, _firstPersonXAxisClamp.y);
            _firstPersonCameraPosition.Transform.localRotation = Quaternion.Euler(xAxisRot, 0f, 0f);

            var rotationShift =
                Quaternion.FromToRotation(mainCamera.transform.forward, _firstPersonCameraPosition.Transform.forward);
            mainCamera.transform.rotation = rotationShift * mainCamera.transform.rotation;

            var rotationAmount = Vector3.Lerp(Vector3.zero,
                new Vector3(0f, 120f * (leftStickInput.x < 0f ? -1f : 1f), 0f),
                Mathf.Abs(leftStickInput.x));
            var deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            player.transform.rotation *= deltaRotation;

            _targetPosition = _firstPersonCameraPosition.Transform.position;

            lookAt = Vector3.Lerp(_targetPosition + player.transform.forward,
                mainCamera.transform.position + mainCamera.transform.forward, 0.1f * Time.deltaTime);
            lookAt = Vector3.Lerp(mainCamera.transform.position + mainCamera.transform.forward, lookAt,
                Vector3.Distance(mainCamera.transform.position, _firstPersonCameraPosition.Transform.position));
        }

        private void RunFreeMode()
        {
            var rigToGoal = _playerOffset - mainCamera.transform.position;
            rigToGoal.y = 0f;

            if (rightStickInput.y < float.PositiveInfinity && rightStickInput.y < -1f * _rightStickThreshold &&
                rightStickInput.y <= _rightStickPrevFrame.y && Mathf.Abs(rightStickInput.x) < _rightStickThreshold)
            {
                distanceUpFree = Mathf.Lerp(DistanceUp, DistanceUp * DistanceUpMultiplier,
                    Mathf.Abs(rightStickInput.y));
                distanceAwayFree = Mathf.Lerp(DistanceAway, DistanceAway * DistanceAwayMultiplier,
                    Mathf.Abs(rightStickInput.y));
                _targetPosition = _playerOffset + player.transform.up * distanceUpFree -
                                  RigToGoalDirection() * distanceAwayFree;
                lastStickMin = rightStickInput.y;
            }
            else if (rightStickInput.y > _rightStickThreshold && rightStickInput.y >= _rightStickPrevFrame.y &&
                     Mathf.Abs(rightStickInput.x) < _rightStickThreshold)
            {
                distanceUpFree = Mathf.Lerp(Mathf.Abs(mainCamera.transform.position.y - _playerOffset.y),
                    _camMinDistanceFromPlayer.y, rightStickInput.y);
                distanceAwayFree = Mathf.Lerp(rigToGoal.magnitude, _camMinDistanceFromPlayer.x, rightStickInput.y);
                _targetPosition = _playerOffset + player.transform.up * distanceUpFree -
                                  RigToGoalDirection() * distanceAwayFree;
                lastStickMin = float.PositiveInfinity;
            }

            if (rightStickInput.x != 0 || rightStickInput.y != 0) savedRigToGoal = RigToGoalDirection();

            mainCamera.transform.RotateAround(_playerOffset, player.transform.up,
                FreeRotationDegreePerSecond *
                (Mathf.Abs(rightStickInput.x) > _rightStickThreshold ? rightStickInput.x : 0f));

            if (_targetPosition == Vector3.zero)
                _targetPosition = _playerOffset + player.transform.up * distanceUpFree -
                                  savedRigToGoal * distanceAwayFree;
        }

        private void RunStandardMode()
        {
            ResetCamera();
            lookDirection = Vector3.Lerp(player.transform.right * (leftStickInput.x < 0 ? 1f : -1f),
                player.transform.forward * (leftStickInput.y < 0 ? -1f : 1f),
                Mathf.Abs(Vector3.Dot(mainCamera.transform.forward, player.transform.forward)));
            currentLookDirection = Vector3.Normalize(_playerOffset - mainCamera.transform.position);
            currentLookDirection.y = 0;
            currentLookDirection =
                Vector3.SmoothDamp(currentLookDirection, lookDirection, ref _velocityLookDir, _lookDirDampTime);
            _targetPosition = _playerOffset + player.transform.up * DistanceUp -
                              Vector3.Normalize(currentLookDirection) * DistanceAway;
        }

        private void RunFocusMode()
        {
            ResetCamera();
            lookDirection = player.transform.forward;
            currentLookDirection = player.transform.forward;
            _targetPosition = _playerOffset + player.transform.up * DistanceUp - lookDirection * DistanceAway;
        }

        public void EnableFocusMode()
        {
            hasStartedFocus = true;
            // appear black bar
            hasBlackBar = true;
            hasStartedFocus = false;
        }

        public void DisableFocusMode()
        {
            hasStartedUnfocus = true;
            // remove black bar
            hasBlackBar = false;
            hasStartedUnfocus = false;
        }

        private Vector3 RigToGoalDirection()
        {
            var rigToGoalDirection = Vector3.Normalize(_playerOffset - mainCamera.transform.position);
            rigToGoalDirection.y = 0f;
            return rigToGoalDirection;
        }

        private void ResetCamera()
        {
            mainCamera.transform.localRotation =
                Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.identity, Time.deltaTime);
        }

        private bool CPositionEligibleForFirstPersonMode()
        {
            return rightStickInput.x < 0.1f && rightStickInput.x > -0.1f && rightStickInput.y > 0.3f;
        }

        private bool CPositionEligibleForFreeMode()
        {
            return rightStickInput.x > 0.1f || rightStickInput.x < -0.1f ||
                   (rightStickInput.y > 0.1f && rightStickInput.y < 0.4f) || rightStickInput.y < -0.1f;
        }

        private enum ECameraState
        {
            STANDARD,
            FOCUS,
            FREE,
            FIRST_PERSON
        }
    }

    internal struct CameraPosition
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
}
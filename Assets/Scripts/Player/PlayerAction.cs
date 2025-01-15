using System;
using UnityEngine;

namespace Player
{
    public class PlayerAction : MonoBehaviour
    {
        public Player player;
        
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Vector2 leftStickInput;
        [SerializeField] private Vector2 rightStickInput;
        [SerializeField] private float playerAngle;
        private const float DefaultMoveSpeed = 1f;
        private const float RotationSpeed = 5f;
        private const float RunSpeedRatio = 7f;


        private readonly PlayerState.EPlayerState[] _freeActionState =
        {
            PlayerState.EPlayerState.STAND,
            PlayerState.EPlayerState.WALK,
            PlayerState.EPlayerState.RUN
        };

        private PlayerControl _playerControl;

        private void Awake()
        {
            ReadInput();
        }

        private void Update()
        {
            if (CanRotate()) Rotate();
            // if (CanStand())
            // {
            //   Stand();
            // }
            if (CanMove()) Move();
            if (CanRoll()) Roll();
        }

        private void FixedUpdate()
        {
            // TODO remttre le contenu de Update() ici une fois corrigé
            if (player.playerState.GetPlayerState() == PlayerState.EPlayerState.RUN ||
                player.playerState.GetPlayerState() == PlayerState.EPlayerState.WALK) MovePlayerByInputAndCamera();
        }

        private void ReadInput()
        {
            _playerControl = new PlayerControl();

            _playerControl.Gameplay.LeftStick.Enable();
            _playerControl.Gameplay.LeftStick.performed += context => { leftStickInput = context.ReadValue<Vector2>(); };
            _playerControl.Gameplay.LeftStick.canceled += _ => leftStickInput = Vector2.zero;
        }

        private void Stand()
        {
            if (moveSpeed > 1f)
            {
                moveSpeed = moveSpeed / 1.7f;
            }
            else
            {
                moveSpeed = 1f;
                player.playerState.SetPlayerState(PlayerState.EPlayerState.STAND);
            }
        }

        private void Move()
        {
            if (CanRun())
                SetIsRunning();
            else if (CanWalk()) SetIsWalking();
        }

        public void Die()
        {
            // TODO
        }

        private void Roll()
        {
            player.playerState.SetPlayerState(PlayerState.EPlayerState.ROLLING);
            // anim
            // boost speed
            // bonk hitbox
            player.playerState.SetPlayerState(PlayerState.EPlayerState.STAND);
        }

        private void Rotate()
        {
            var forward = player.playerCamera.mainCamera.transform.forward;
            var right = player.playerCamera.mainCamera.transform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            var movementDirection = (forward * leftStickInput.y + right * leftStickInput.x).normalized;
            var targetRotation = Quaternion.LookRotation(movementDirection);
            player.transform.rotation =
                Quaternion.Slerp(player.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        private void SetIsRunning()
        {
            moveSpeed = RunSpeedRatio;
            player.playerState.SetPlayerState(PlayerState.EPlayerState.RUN);
        }

        private void SetIsWalking()
        {
            var highestAnalogInput = Math.Abs(leftStickInput.y) > Math.Abs(leftStickInput.x)
                ? Math.Abs(leftStickInput.y)
                : Math.Abs(leftStickInput.x);
            moveSpeed = highestAnalogInput * 10;
            player.playerState.SetPlayerState(PlayerState.EPlayerState.WALK);
        }

        private bool CanRun()
        {
            return leftStickInput.y > 0.5f || leftStickInput.y < -0.5f || leftStickInput.x > 0.5f ||
                   leftStickInput.x < -0.5f;
        }

        private bool CanWalk()
        {
            return (leftStickInput.y < 0.5f && leftStickInput.y > 0.02f) ||
                   (leftStickInput.y > -0.5f && leftStickInput.y < -0.02f) ||
                   (leftStickInput.x < 0.5f && leftStickInput.x > 0.02f) ||
                   (leftStickInput.x > -0.5f && leftStickInput.x < -0.02f);
        }

        private bool CanStand()
        {
            return Array.Exists(_freeActionState, element => element == player.playerState.GetPlayerState()) &&
                   leftStickInput.y < 0.02f && leftStickInput.y > -0.02f && leftStickInput.x < 0.02f &&
                   leftStickInput.x > -0.02f;
        }

        private bool CanMove()
        {
            return Array.Exists(_freeActionState, element => element == player.playerState.GetPlayerState());
        }

        private bool CanRoll()
        {
            return CanMove() && CanRun() && Input.GetKeyDown(KeyCode.H); // TODO new input system
        }

        private bool CanRotate()
        {
            return Array.Exists(_freeActionState, element => element == player.playerState.GetPlayerState()) &&
                   (leftStickInput.y != 0f || leftStickInput.x != 0f);
        }

        private void MovePlayerByInputAndCamera()
        {
            var stickDirection = new Vector3(leftStickInput.x, 0, leftStickInput.y);

            var cameraDirection = player.playerCamera.mainCamera.transform.forward;
            cameraDirection.y = 0.0f;
            var referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(cameraDirection));

            var moveDirection = referentialShift * stickDirection;
            if (!(moveDirection.magnitude > 0.1f)) return;
            var newPosition = player.transform.position + moveDirection * (moveSpeed * Time.deltaTime);
            player.rigidbody.MovePosition(newPosition);
        }

        public void SpawnAt(Transform transformSpawn)
        {
            player.transform.rotation = transformSpawn.rotation;
            player.transform.position = transformSpawn.position;
        }

        private void ResetSpeed()
        {
            moveSpeed = DefaultMoveSpeed;
        }

        public void SetupReadMode()
        {
            player.playerState.SetPlayerState(PlayerState.EPlayerState.STAND);
            // animation stand
            ResetSpeed();
            // camera int
        }

        public void ExitReadMode()
        {
            player.playerState.SetPlayerState(PlayerState.EPlayerState.STAND);
            // animation stand
            ResetSpeed();
            // camera out
        }
    }
}
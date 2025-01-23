using Actors.Ables;
using Actors.Handlers;
using Manager;
using Player;
using UnityEngine;

namespace Actors
{
    public class TestActor : MonoBehaviour
    {
        private ISpawnResetable _spawnResetable;
        private bool _isRunning;

        private void Awake()
        {
            _spawnResetable = new ISpawnResetable(transform);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W) && _isRunning == false)
            {
                _isRunning = true;
                PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                    .FADE_IN_BLACK);
            }

            if (Input.GetKey(KeyCode.X) && _isRunning == false)
            {
                _isRunning = true;
                PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                    .FADE_OUT_BLACK);
            }

            if (Input.GetKey(KeyCode.C) && _isRunning == false)
            {
                _isRunning = true;
                PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                    .FADE_IN_WHITE);
            }

            if (Input.GetKey(KeyCode.V) && _isRunning == false)
            {
                _isRunning = true;
                PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                    .FADE_OUT_WHITE);
            }

            if (!PlayerManager.Instance.player.playerCameraEffect.transitionIsRunning) _isRunning = false;
        }

        protected void OnDisable()
        {
            _spawnResetable.ResetToSpawnPosition();
            _isRunning = false;
        }
    }
}

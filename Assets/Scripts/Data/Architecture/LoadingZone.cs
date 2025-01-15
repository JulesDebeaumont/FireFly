using Manager;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Architecture
{
    public class LoadingZone : MonoBehaviour
    {
        public enum ELoadingZoneCameraTransition
        {
            FADE_IN_BLACK,
            FADE_IN_WHITE
        }

        [FormerlySerializedAs("_boxCollider")] [SerializeField] private BoxCollider boxCollider;
        [FormerlySerializedAs("SceneDestinationId")] public int sceneDestinationId;
        [FormerlySerializedAs("SceneDestionationSpawnId")] public int sceneDestionationSpawnId;
        [FormerlySerializedAs("CameraTransition")] public ELoadingZoneCameraTransition cameraTransition = ELoadingZoneCameraTransition.FADE_IN_BLACK;
        private bool _hasBeenTriggered;
        private bool _isLoading;

        private void Update()
        {
            var playerTouchLoadingZone = boxCollider.bounds.Intersects(PlayerManager.Instance.player.collider.bounds);
            if (playerTouchLoadingZone && !_hasBeenTriggered && !_isLoading)
            {
                // TODO Lock Camera position, and maybe rotation too
                StartTransition();
                return;
            }

            if (_hasBeenTriggered && !PlayerManager.Instance.player.playerCameraEffect.transitionIsRunning && !_isLoading)
            {
                _isLoading = true;
                LoadNext();
            }
        }

        private void StartTransition()
        {
            _hasBeenTriggered = true;
            switch (cameraTransition)
            {
                case ELoadingZoneCameraTransition.FADE_IN_BLACK:
                    PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                        .FADE_IN_BLACK);
                    break;

                case ELoadingZoneCameraTransition.FADE_IN_WHITE:
                    PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                        .FADE_IN_WHITE);
                    break;
            }
        }

        private void LoadNext()
        {
            SceneCustomManager.Instance.LoadScene(sceneDestinationId, sceneDestionationSpawnId);
        }
    }
}
using Manager;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Architecture
{
    public class SceneSpawn : MonoBehaviour
    {
        public enum ESpawnCameraTransition
        {
            FADE_OUT_BLACK,
            FADE_OUT_WHITE
        }

        public enum ESpawnPlayerAnimation
        {
        }

        public GameObject gameObject;
        public int id;
        public int sceneRoomId;
        public ESpawnPlayerAnimation spawnPlayerAnimation;
        public ESpawnCameraTransition cameraTransition;
        private bool _hasBeenTriggered;

        private void Start()
        {
            PlayerManager.Instance.player.playerAction.SpawnAt(transform);
            switch (cameraTransition)
            {
                case ESpawnCameraTransition.FADE_OUT_BLACK:
                    PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                        .FADE_OUT_BLACK);
                    break;

                case ESpawnCameraTransition.FADE_OUT_WHITE:
                    PlayerManager.Instance.player.playerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition
                        .FADE_OUT_WHITE);
                    break;
            }

            _hasBeenTriggered = true;
        }
    }
}
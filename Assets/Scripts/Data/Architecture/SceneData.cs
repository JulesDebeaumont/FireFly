using Manager;
using UnityEngine;

// https://discussions.unity.com/t/room-system-at-runtime/1539537/4
namespace Data.Architecture
{
    public class SceneData : MonoBehaviour
    {
        public int worldMapIndex;
        public GameObject spawnList;
        public GameObject roomList;

        private void Awake()
        {
            var spawns = spawnList.GetComponentsInChildren<SceneSpawn>(false);
            var roomIdToLoad = 0;
            foreach (var spawn in spawns)
                if (spawn.id == SceneCustomManager.Instance.currentSpawnId)
                    roomIdToLoad = spawn.sceneRoomId;
                else
                    Destroy(spawn.gameObject);

            UnsetUnusedRoom(roomIdToLoad);
            SceneCustomManager.Instance.currentScene = this;
            SceneCustomManager.Instance.currentSceneRoomId = roomIdToLoad;
        }

        public void LoadRoom(int roomId)
        {
            var rooms = roomList.GetComponentsInChildren<SceneRoom>(false);
            foreach (var room in rooms)
                if (room.id == roomId)
                {
                    room.gameObject.SetActive(true);
                    return;
                }
        }

        public void UnloadRoom(int roomId)
        {
            var rooms = roomList.GetComponentsInChildren<SceneRoom>(false);
            foreach (var room in rooms)
                if (room.id == roomId)
                {
                    room.gameObject.SetActive(false);
                    return;
                }
        }

        private void UnsetUnusedRoom(int roomIdToKeepActive)
        {
            var rooms = roomList.GetComponentsInChildren<SceneRoom>(false);
            foreach (var room in rooms)
                if (room.id != roomIdToKeepActive)
                    room.gameObject.SetActive(false);
        }
    }
}
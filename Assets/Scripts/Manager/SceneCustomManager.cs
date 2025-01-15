using Data.Architecture;
using Data.Tables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Manager
{
    public class SceneCustomManager : MonoBehaviour
    {
        [FormerlySerializedAs("CurrentScene")] public SceneData currentScene;
        [FormerlySerializedAs("CurrentSceneId")] public int currentSceneId;
        [FormerlySerializedAs("CurrentSpawnId")] public int currentSpawnId;
        [FormerlySerializedAs("CurrentSceneRoomId")] public int currentSceneRoomId;
        public static SceneCustomManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }


        public void LoadScene(int sceneId, int sceneSpawnId)
        {
            var sceneToLoad = SceneTable.GetSceneNameById(sceneId);
            SceneManager.LoadScene(sceneToLoad);
            currentSceneId = sceneId;
            currentSpawnId = sceneSpawnId;
            // TODO check for world flag, special event redirect
        }
    }
}
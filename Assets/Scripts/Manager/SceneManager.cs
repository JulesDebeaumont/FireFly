using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCustomManager : MonoBehaviour
{
    public static SceneCustomManager Instance { get; private set; }
    public SceneData CurrentScene;
    public int CurrentSceneId;
    public int CurrentSpawnId;
    public int CurrentSceneRoomId;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void LoadScene(int sceneId, int sceneSpawnId)
    {
      var sceneToLoad = SceneTable.GetSceneNameById(sceneId);
      SceneManager.LoadScene(sceneToLoad);
      CurrentSceneId = sceneId;
      CurrentSpawnId = sceneSpawnId;
      // TODO check for world flag, special event redirect
    }
}


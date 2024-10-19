using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    public SceneData CurrentScene;
    public int CurrentSceneId;
    public int CurrentSetupId;
    public int CurrentSpawnId;
    public int CurrentSceneRoomId;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {is
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void LoadScene(int sceneId, int sceneSpawnId, int setupId = 0)
    {
      var sceneToLoad = SceneTable.GetSceneNameById(sceneId);
      SceneManagement.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
      CurrentSceneId = sceneId;
      CurrentSpawnId = sceneSpawnId;
      CurrentSetupId = setupId;
      // TODO method to calculate setup, like worldflag or day/night stuff ?
    }
}


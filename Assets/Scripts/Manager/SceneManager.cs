using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    public SceneData CurrentScene;
    public int CurrentSceneId;
    public int CurrentSpawnId;
    public int CurrentSetupId;

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

    public void LoadScene(string sceneName)
    {
      // wait a bit before loading for not-too-fast load, keep the odl n64 delay
      // TODO
      // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
      // find the SceneData GameObject to put in the CurrentScene variable
    }

    public void LoadSceneWithRequestedSetup(string sceneName, int setupId)
    {
      // TODO
    }
}


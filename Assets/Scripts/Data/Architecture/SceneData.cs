using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
  public List<SceneSetup> SceneSetupList = new();
  public List<SceneSpawn> SpawnList = new();
  public LoadingZone[] LoadingZones = new();
  public List<SceneRoomMesh> SceneRoomMeshes = new();
  public SceneRoomTransition[] SceneRoomTransitions = new();
  public int WorldMapIndex;
  // TODO Collision

  void Awake()
  {
    // Look for spawn with id from SceneManager
    var spawn = SpawnList.Find(spawnFind => spawnFind.Id == SceneManager.Instance.CurrentSpawnId);

    // Load RoomMesh prefab with RoomId from spawnId
    LoadRoomMesh(spawn.SceneRoomId);

    // get sceneSetup prefab
    var setup = SceneSetupList.Find(setupFind => setupFind.Id == SceneManager.Instance.CurrentSetupId);
    Instanciate(setup); // TODO
    Instanciate(spawn); // TODO
  }

  public void LoadRoomMesh(int roomMeshId)
  {
    var roomMesh = SceneRoomMeshes.Find(roomMeshFind => roomMeshFind.Id == roomMeshId);
     Instanciate(roomMesh); // TODO 
  }

  private SceneSetup GetSceneSetup()
  {
    return SceneSetupList.FirstOrDefault(setup => setup.Id == SceneManager.Instance.CurrentSetupId);
  }
}

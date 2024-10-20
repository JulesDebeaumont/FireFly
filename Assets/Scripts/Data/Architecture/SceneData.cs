using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneData : MonoBehaviour
{
  public List<SceneSpawn> SpawnList;
  public LoadingZone[] LoadingZones;
  public List<SceneRoom> SceneRoomList;
  public SceneRoomTransition[] SceneRoomTransitions;
  public int WorldMapIndex;
  // TODO Collision
  // https://discussions.unity.com/t/room-system-at-runtime/1539537/4

  void Awake()
  {
    // Look for spawn with id from SceneManager
    var spawn = SpawnList.Find(spawnFind => spawnFind.Id == SceneCustomManager.Instance.CurrentSpawnId);

    // Load RoomMesh prefab with RoomId from spawnId
    LoadRoom(spawn.SceneRoomId);

    // get sceneSetup prefab
    // Instantiate(spawn); // TODO
  }

  public void LoadRoom(int roomId)
  {
    var roomMesh = SceneRoomList.Find(roomFind => roomFind.Id == SceneCustomManager.Instance.CurrentSceneRoomId);
     Instantiate(roomMesh); // TODO 
  }
}

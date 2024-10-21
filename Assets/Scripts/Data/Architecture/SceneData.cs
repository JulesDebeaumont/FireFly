using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// https://discussions.unity.com/t/room-system-at-runtime/1539537/4
public class SceneData : MonoBehaviour
{
  public int WorldMapIndex;
  public GameObject SpawnList;
  public GameObject RoomList;

  void Awake()
  {
    var spawns = SpawnList.GetComponentsInChildren<SceneSpawn>(false);
    int roomIdToLoad = 0;
    foreach (SceneSpawn spawn in spawns)
    {
      if (spawn.Id == SceneCustomManager.Instance.CurrentSpawnId)
      {
        roomIdToLoad = spawn.SceneRoomId;
      }
      else
      {
        Destroy(spawn.GameObject);
      }
    }
    UnsetUnusedRoom(roomIdToLoad);
    SceneCustomManager.Instance.CurrentScene = this;
    SceneCustomManager.Instance.CurrentSceneRoomId = roomIdToLoad;
  }

  public void LoadRoom(int roomId)
  {
    var rooms = RoomList.GetComponentsInChildren<SceneRoom>(false);
    foreach(SceneRoom room in rooms)
    {
      if (room.Id == roomId)
      {
        room.GameObject.SetActive(true);
        return;
      }
    }
  }

  public void UnloadRoom(int roomId)
  {
    var rooms = RoomList.GetComponentsInChildren<SceneRoom>(false);
    foreach(SceneRoom room in rooms)
    {
      if (room.Id == roomId)
      {
        room.GameObject.SetActive(false);
        return;
      }
    }
  }

  private void UnsetUnusedRoom(int roomIdToKeepActive)
  {
    var rooms = RoomList.GetComponentsInChildren<SceneRoom>(false);
    foreach(SceneRoom room in rooms)
    {
      if (room.Id != roomIdToKeepActive)
      {
        room.GameObject.SetActive(false);
      }
    }
  }

}

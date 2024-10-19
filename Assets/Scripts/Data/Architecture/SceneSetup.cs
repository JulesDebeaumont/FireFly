using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
  public int Id;
  public List<Pathway> PathwayList = new();
  public List<SceneRoom> RoomList = new();

  void Awake()
  {
    var room = RoomList.Find(roomFind => roomFind.Id == SceneManager.Instance.CurrentSceneRoomId);
    Instanciate(room);
  }
}

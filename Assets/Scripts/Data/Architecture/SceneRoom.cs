using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoom : MonoBehaviour
{
  public int Id;
  public int SceneRoomMeshId;
  public List<Actor> ActorList = new();

  private void LoadActors()
  {

  }

  private void UnloadActors()
  {

  }

  private void LoadMesh()
  {

  }

  private void UnloadMesh()
  {

  }

  public void Load()
  {
    LoadActors();
    LoadMesh();
  }

  public void Unload()
  {
    UnloadActors();
    UnloadMesh();
  }
}

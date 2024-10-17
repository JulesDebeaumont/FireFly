using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
  public int Id;
  public List<SceneSetup> SceneSetupList = new();
  public List<SceneSpawn> SpawnList = new();
  public List<LoadingZone> LoadingZoneList = new();
  public List<SceneRoomMesh> SceneRoomMeshList = new();
  public List<SceneTransition> SceneTransitionList = new();
  public int WorldMapIndex;
  // TODO Collision

  protected virtual int DetermineSetupIndexToLoad()
  {
    return 0;
    // Used when loading scene to check which setup to use
    // Should be inherited
  }

  public SceneSetup GetSceneSetup() // Call dans le scene manager après avoir loader la scene, ensuite c'est le setup qui load le reste
  {
    return SceneSetupList[DetermineSetupIndexToLoad()];
  }

  // TODO attacher le SceneData à une gameobject root dans la scene au même niveau que le MainContext prefab
  // donc MonoBehaviour ?
}

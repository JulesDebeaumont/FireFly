using System.Collections;
using System.Collections.Generic;

public static class SceneTable
{
  public static string GetSceneNameById(int sceneId)
  {
    if (SceneList.TryGetValue(sceneId, out string sceneName))
    {
      return sceneName;
    }
    return SceneList[0];
  }

  private static Dictionary<int, string> SceneList = new()
  {
    {0, "TestScene"}
  };
}

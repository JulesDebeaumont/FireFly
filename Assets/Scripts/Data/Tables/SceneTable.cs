using System.Collections;
using System.Collections.Generic;

public static class SceneTable
{
  private static Dictionary<int, string> SceneList = new()
  {
    {0, "TestScene"}
  };

  public static string GetSceneNameById(int sceneId)
  {
    if (!SceneList.TryGetValue(sceneId, out string sceneName))
    {
      return SceneList[0];
    }
    return sceneName;
  }
}

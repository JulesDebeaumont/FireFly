using System.Collections.Generic;

namespace Data.Tables
{
    public static class SceneTable
    {
        private static readonly Dictionary<int, string> SceneList = new()
        {
            { 0, "TestScene" }
        };

        public static string GetSceneNameById(int sceneId)
        {
            if (SceneList.TryGetValue(sceneId, out var sceneName)) return sceneName;
            return SceneList[0];
        }
    }
}
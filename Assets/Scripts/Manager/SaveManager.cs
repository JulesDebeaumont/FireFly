using System.Collections.Generic;
using System.IO;
using Player;
using UnityEngine;

namespace Manager
{
    public static class SaveManager
    {
        public static int CurrentFileLoaded = 0;

        public static void WriteSaveFile(SaveFileFormat save)
        {
            var jsonPlayerSave = JsonUtility.ToJson(save);
            File.WriteAllText(GetSaveFilePath(save.Id), jsonPlayerSave);
        }

        public static void CreateSaveFile(int index)
        {
            var newFile = new SaveFileFormat { Id = index };
            WriteSaveFile(newFile);
        }

        public static SaveFileFormat ReadSaveFile(int index)
        {
            return JsonUtility.FromJson<SaveFileFormat>(File.ReadAllText(GetSaveFilePath(index)));
        }

        public static SaveFileFormat ReadCurrentSaveFile()
        {
            return ReadSaveFile(CurrentFileLoaded);
        }

        private static string GetSaveFilePath(int index)
        {
            return Path.Combine("Assets", "PlayerSaveFiles", index.ToString(), ".json");
        }

        public record SaveFileFormat
        {
            public int CurrentSceneId;
            public int CurrentSceneSpawnId;
            public int Id;
            public Dictionary<int, bool[]> SceneFlags = new();
            public Dictionary<PlayerFlag.EWorldFlagType, bool> WorldFlags = new();
        }
    }
}
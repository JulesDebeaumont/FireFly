using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        var newFile = new SaveFileFormat() { Id = index };
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
        return Path.Combine(Path.Combine(["Assets", "PlayerSaveFiles"]), $"{index}.json");
    }

    public class SaveFileFormat
    {
        public int Id;
        public Dictionnary<int, bool[]> SceneFlags = new();
        public Dictionnary<PlayerFlag.EWorldFlagType, bool> WorldFlags = new();
        public int CurrentSceneId;
        public int CurrentSceneSpawnId;
    }
}

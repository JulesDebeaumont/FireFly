using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlag
{
    public Player Player;
    private Dictionnary<int, bool[]> _sceneFlags = new ();
    private Dictionnary<EWorldFlagType, bool> _worldFlags = new ();

    public PlayerFlag()
    {
        LoadFlagFromSave()
    }

    private void LoadFlagFromSave()
    {
        SaveManager.CreateSaveFile(0); // TODO remove once file is created
        var saveFile = SaveManager.ReadCurrentSaveFile();
        _sceneFlags = saveFile.SceneFlags;
        _worldFlags = saveFile.WorldFlags;
    }

    public bool IsSceneFlagSet(int sceneId, int index)
    {
        return _sceneFlags[sceneId][index];
    }

    public void SetSceneFlag(int sceneId, int index)
    {
        _sceneFlags[sceneId][index] = true;
    }

    public void UnsetSceneFlag(int sceneId, int index)
    {
        _sceneFlags[sceneId][index] = false;
    }

    public void SetWorldFlag(EWorldFlagType type)
    {
        _worldFlags[type] = true;
    }

    public void UnsetWorldFlag(EWorldFlagType type)
    {
        _worldFlags[type] = false;
    }

    public bool IsWorldFlagSet(EWorldFlagType type)
    {
        return _worldFlags[type];
    }

    public enum EWorldFlagType
    {
        OPENING_DONE
    }
}

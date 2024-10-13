using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class FlagTable
{
    private Dictionnary<int, bool[]> SceneFlags = new Dictionnary<FlagTable, bool>();
    private Dictionnary<EWorldFlagType, bool> WorldFlags = new Dictionnary<EWorldFlagType, bool>();
    public FlagTable()
    {
        SetupSave();
    }

    private void SetupSave()
    {
        // TODO fetch in file
    }

    public bool IsSceneFlagSet(int sceneId, int index)
    {
        return SceneFlags[sceneId][index];
    }

    public bool IsWorldFlagSet(EWorldFlagType type)
    {
        return WorldFlags[type];
    }

    public enum EWorldFlagType
    {
        OPENING_DONE
    }
}

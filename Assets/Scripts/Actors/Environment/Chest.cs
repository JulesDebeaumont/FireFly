using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : EnvironmentActor
{
    public CollectibleItem CollectibleItem;
    public int FlagId = 0;
    public bool HasBeenLooted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (FlagId != 0 && PlayerManager.Instance.Player.PlayerFlag.IsCurrentSceneFlagSet(FlagId))
        {
            HasBeenLooted = true;
        }
    }

    void Update()
    {
        // detect player in front of chest
        // if player open
        
    }

}

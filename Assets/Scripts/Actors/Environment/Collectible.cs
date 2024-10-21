using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Actor
{
    public CollectibleItem CollectibleItem;
    public int FlagId = 0;
    public bool HopAtSpawn = false;
    private bool _hasHop = false;

    // Start is called before the first frame update
    void Start()
    {
        if (FlagId != 0 && PlayerManager.Instance.Player.PlayerFlag.IsCurrentSceneFlagSet(FlagId))
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Update()
    {
        if (HopAtSpawn == false)
        {
            StandAndRotate();
        }
        else
        {
            if (_hasHop == false)
            {
                Hop();
            }
        }

        // TODO detect collision with player
        if (false)
        {
            Collect();
        }
    }

    public void Collect()
    {
        // anim

        // TODO put this in Player.cs
        /*
        PlayerManager.Instance.PlayerState.SetPlayerState(PlayerState.EPlayerState.LOOTING);
        if (this.CollectibleItem.Animation == PlayerState.ECollectAnimation.NONE)
        {
            // SoundManager.Instance.PlaySoundEffect(this.CollectibleItem.SoundEffect);
            // TODO
        }
        else
        {
            // wait for camera and animation
            DialogManager.Instance.DisplayDialog(this.CollectibleItem.Dialog);
        }
        this.CollectibleItem.Collect();
        */
        if (FlagId != 0)
        {
            SetFlag();
        }
        Destroy(gameObject);
    }

    private void StandAndRotate()
    {
        // TODO
    }

    private void SetFlag()
    {
        PlayerManager.Instance.Player.PlayerFlag.SetCurrentSceneFlag(FlagId);
    }

    private void Hop()
    {
        // TODO
        _hasHop = true;
    }

}

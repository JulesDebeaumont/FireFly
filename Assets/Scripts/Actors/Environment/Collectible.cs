using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : EnvironmentActor
{
    public CollectibleItem CollectibleItem;
    public int Flag = 0;
    public bool HopAtSpawn = false;
    private bool HasHop = false;

    // Start is called before the first frame update
    void Start()
    {
        if (this.Flag != 0 && SceneManager.Instance.IsFlagSet(this.Flag))
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Update()
    {
        if (this.HopAtSpawn == false)
        {
            StandAndRotate();
        }
        else
        {
            if (this.HasHop == false)
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
        PlayerManager.Instance.PlayerState.State = PlayerState.EPlayerState.LOOTING;
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
        if (this.Flag != 0)
        {
            SetFlag();
        }
        Destroy(this.gameObject);
    }

    private void StandAndRotate()
    {
        // TODO
    }

    private void SetFlag()
    {
        SceneManager.Instance.SetFlag(this.Flag);
    }

    private void Hop()
    {
        // TODO
        this.HasHop = true;
    }

}

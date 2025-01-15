using Actors.Environments.CollectibleItems;
using Manager;
using UnityEngine;

namespace Actors.Environment
{
    public class Collectible : MonoBehaviour
    {
        public int flagId;
        public bool hopAtSpawn;
        private bool _hasHop;
        public CollectibleItem CollectibleItem;

        // Start is called before the first frame update
        private void Start()
        {
            if (flagId != 0 && PlayerManager.Instance.player.PlayerFlag.IsCurrentSceneFlagSet(flagId)) Destroy(gameObject);
        }

        private void Update()
        {
            if (hopAtSpawn == false)
            {
                StandAndRotate();
            }
            else
            {
                if (_hasHop == false) Hop();
            }

            // TODO detect collision with player
            if (false) Collect();
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
            if (flagId != 0) SetFlag();
            Destroy(gameObject);
        }

        private void StandAndRotate()
        {
            // TODO
        }

        private void SetFlag()
        {
            PlayerManager.Instance.player.PlayerFlag.SetCurrentSceneFlag(flagId); // TODO handler
        }

        private void Hop()
        {
            // TODO
            _hasHop = true;
        }
    }
}
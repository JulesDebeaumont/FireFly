using Actors.Environments.CollectibleItems;
using Manager;

namespace Actors.Environment
{
    public class Chest : Actor
    {
        public int flagId;
        public bool hasBeenLooted;
        public CollectibleItem CollectibleItem;

        // Start is called before the first frame update
        private void Start()
        {
            if (flagId != 0 && PlayerManager.Instance.player.PlayerFlag.IsCurrentSceneFlagSet(flagId)) hasBeenLooted = true;
        }

        private void Update()
        {
            // detect player in front of chest
            // if player open
        }
    }
}
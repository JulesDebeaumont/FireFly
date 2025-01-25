using Actors.Environments.CollectibleItems;
using Manager;

namespace Data.Definitions.CollectibleItems
{
    public class Heart : ICollectibleItem
    {
        public void Collect()
        {
            PlayerManager.Instance.player.playerInventory.AddAmber(1);
        }
    }
}
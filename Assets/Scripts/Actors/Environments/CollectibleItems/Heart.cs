using Manager;

namespace Actors.Environments.CollectibleItems
{
    public class Heart : CollectibleItem
    {
        public new void AddToInventory()
        {
            PlayerManager.Instance.player.playerInventory.AddAmber(1);
        }
    }
}
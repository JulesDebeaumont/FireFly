using Manager;

namespace Actors.Environments.CollectibleItems
{
    public abstract class CollectibleItem
    {
        public void AddToInventory()
        {
            PlayerManager.Instance.player.playerInventory.AddAmber(1);
        }
    }
}

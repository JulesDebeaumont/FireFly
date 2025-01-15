using Manager;

namespace Actors.Environments.CollectibleItems
{
    public class SmallAmber : CollectibleItem
    {
        public new void AddToInventory()
        {
            PlayerManager.Instance.player.playerInventory.AddAmber(1);
        }
    }
}

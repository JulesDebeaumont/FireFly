using Manager;

namespace Actors.Environments.CollectibleItems
{
    public class BigAmber : CollectibleItem
    {
        public new void AddToInventory()
        {
            PlayerManager.Instance.player.playerInventory.AddAmber(20);
        }
    }
}

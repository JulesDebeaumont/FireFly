using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using UnityEngine;

namespace Actors.MonoHandlers
{
    public class DropMonoHandler : MonoBehaviour
    {
        private DropTable _dropTable;

        public void Initialize(DropTable dropTable)
        {
            _dropTable = dropTable;
        }
        
        public void PickAndSpawn(Vector3 position, EDropSpawnAnimation animation)
        {
            var collectible = _dropTable.Pick();
            if (collectible == null) return;
            SpawnCollectible(collectible, position, animation);
        }

        private void Update()
        {
            // TODO
        }
        
        private void SpawnCollectible(CollectibleItem collectible, Vector3 position, EDropSpawnAnimation animation)
        {
            switch (animation)
            {
                case EDropSpawnAnimation.STAND:
                    // TODO
                    break;
                
                case EDropSpawnAnimation.HOP:
                    // TODO
                    break;
                
                case EDropSpawnAnimation.BIG_HOP:
                    // TODO
                    break;
            }
        }
        
    }
}
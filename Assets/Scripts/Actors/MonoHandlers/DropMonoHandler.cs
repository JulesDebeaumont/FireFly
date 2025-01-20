using Actors.Definitions;
using UnityEngine;

namespace Actors.MonoHandlers
{
    public class DropMonoHandler : MonoBehaviour
    {
        private readonly DropTable _dropTable;
        
        public DropMonoHandler(DropTable dropTable)
        {
            _dropTable = dropTable;
        }

        public void Initialize()
        {
            
        }
        
        public void PickAndSpawn(Vector3 position, EDropSpawnAnimation animation)
        {
            if (!EnsureDropIsAllowed()) return;
            SpawnCollectible(position, animation);
        }
        
        // TODO Refacto so the droptable is automatically altered by player inventory and status instead of picking nothing ?
        // then remove method I guess
        private bool EnsureDropIsAllowed()
        {
            return true;
        }

        private void SpawnCollectible(Vector3 position, EDropSpawnAnimation animation)
        {
            // TODO
            // switch ESpawnAnimation
        }
        
    }
}
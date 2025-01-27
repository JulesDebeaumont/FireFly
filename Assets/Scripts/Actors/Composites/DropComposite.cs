using Actors.Definitions;
using Actors.Environments;
using Data.Definitions;
using Data.Tables;
using UnityEngine;

namespace Actors.Composites
{
    public class DropComposite
    {
        private Transform _transform;
        
        public DropComposite(Transform transform)
        {
            _transform = transform;
        }
        
        public void Drop(ECollectibleType collectibleType, EDropSpawnAnimation animation)
        {
           var drop = GameObject.Instantiate(CollectibleTable.Get(collectibleType), _transform.position, _transform.rotation);
           drop.GetComponent<Collectible>().spawnAnimation = animation;
        }

        public void DropRandom(DropTable dropTable, EDropSpawnAnimation animation)
        {
            var pickedType = dropTable.Pick();
            if (pickedType == null) return;
            var drop = GameObject.Instantiate(CollectibleTable.Get(pickedType.Value), _transform.position, _transform.rotation);
            drop.GetComponent<Collectible>().spawnAnimation = animation;
        }
    }
}

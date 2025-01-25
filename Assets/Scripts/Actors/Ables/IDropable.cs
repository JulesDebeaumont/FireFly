using System;
using Actors.Definitions;
using Actors.Environments;
using Data.Tables;
using UnityEngine;

namespace Actors.Ables
{
    public interface IDropable
    {
        Transform Transform { get; }
        GameObject Instanciate(GameObject collectiblePrefab, Vector3 position, Quaternion rotation);

        private void Drop(Type collectibleType, EDropSpawnAnimation animation)
        {
           var drop = Instanciate(CollectibleTable.Get(collectibleType), Transform.position, Transform.rotation);
           drop.GetComponent<Collectible>().spawnAnimation = animation;
        }

        private void DropRandom(DropTable dropTable, EDropSpawnAnimation animation)
        {
            var drop = Instanciate(CollectibleTable.Get(dropTable.Pick()), Transform.position, Transform.rotation);
            drop.GetComponent<Collectible>().spawnAnimation = animation;
        }
    }
}

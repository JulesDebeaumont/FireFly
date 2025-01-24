using System;
using Actors.Definitions;
using Manager;
using UnityEngine;

namespace Actors.Ables
{
    public interface IDropable
    {
        bool IsDropping { get; set; }
        EDropSpawnAnimation DropAnimation { get; set; }
        DropTable Droptable { get; }
        Transform Transform { get; }
        int GetInstanceId();

        public void Drop()
        {
            DropableManager.Instance.RegisterEntry(this);
        }

        void UpdateDropable() // TODO
        {
            if (!IsDropping)
            {
                PickAndSpawn();
            }
        }
                
        private void PickAndSpawn()
        {
            var collectibleType = Droptable.Pick();
            if (collectibleType == null)
            {
                IsDropping = false;
            }
        }
        
        private void SpawnCollectible(Type collectibleType)
        {
            switch (DropAnimation)
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

using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Environments
{
    public class Collectible : MonoBehaviour
    {
        public int flagId;
        public EDropSpawnAnimation spawnAnimation = EDropSpawnAnimation.STAND;
        public ICollectibleItem CollectibleItem; // TODO look for a way to setup in unity inspector
        public Rigidbody rigidbody;

        public void Collect()
        {
            CollectibleItem.Collect();
        }

        private void Awake()
        {
            SpawnAnimation();
        }

        private void Update()
        {
            // TODO
        }
        
        private void SpawnAnimation()
        {
            var force = 0f;
            switch (spawnAnimation)
            {
                case EDropSpawnAnimation.STAND:
                    return;
                
                case EDropSpawnAnimation.HOP:
                    force = 3f;
                    break;
                
                case EDropSpawnAnimation.BIG_HOP:
                    force = 5f;
                    break;
            }
            var randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
            rigidbody.AddForce(randomDirection * force, ForceMode.Impulse);
        }
    }
}
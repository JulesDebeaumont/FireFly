using Actors.Composites;
using Actors.Environments.CollectibleItems;
using UnityEngine;

namespace Actors.Environments
{
    public class Chest : MonoBehaviour
    {
        public int flagId;
        public bool _hasBeenLooted;
        public ICollectibleItem CollectibleItem;

        private void Awake()
        {
        }

        private void OnDisable()
        {
        }

        private void Start()
        {
        }

        private void Update()
        {
            // detect player in front of chest
            // if player open
        }
    }
}
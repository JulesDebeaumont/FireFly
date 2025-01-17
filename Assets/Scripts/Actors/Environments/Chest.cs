using Actors.Environments.CollectibleItems;
using Actors.Handlers;
using UnityEngine;

namespace Actors.Environments
{
    public class Chest : MonoBehaviour
    {
        private FlagHandler _flagHandler;
        private SpawnResetHandler _spawnResetHandler;
        public int flagId;
        public bool _hasBeenLooted;
        public CollectibleItem CollectibleItem;

        private void Awake()
        {
            _spawnResetHandler = new SpawnResetHandler(transform);
            _flagHandler = new FlagHandler(flagId);
            if (_flagHandler.IsCurrentSceneFlagSet()) _hasBeenLooted = true;
        }

        private void OnDisable()
        {
            _spawnResetHandler.ResetToSpawnPosition();
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
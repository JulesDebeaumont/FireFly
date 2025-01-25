using Actors.Ables;
using Actors.Environments.CollectibleItems;
using UnityEngine;

namespace Actors.Environments
{
    public class Chest : MonoBehaviour
    {
        private FlagHandler _flagHandler;
        private ISpawnResetable _spawnResetable;
        public int flagId;
        public bool _hasBeenLooted;
        public ICollectibleItem CollectibleItem;

        private void Awake()
        {
            _spawnResetable = new ISpawnResetable(transform);
            _flagHandler = new FlagHandler(flagId);
            if (_flagHandler.IsCurrentSceneFlagSet()) _hasBeenLooted = true;
        }

        private void OnDisable()
        {
            _spawnResetable.ResetToSpawnPosition();
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
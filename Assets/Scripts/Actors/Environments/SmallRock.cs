using System;
using System.Collections.Generic;
using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using Actors.Handlers;
using Actors.MonoHandlers;
using UnityEngine;

namespace Actors.Environments
{
    /*
     * Liftable small rock
     */
    public class SmallRock : MonoBehaviour
    {
        private SpawnResetHandler _spawnResetHandler;
        private FlagHandler _flagHandler;
        private static readonly DropMonoHandler DropMonoHandler = new( new Dictionary<Type, int>
        {
            { typeof(Heart), 20 },
            { typeof(SmallAmber), 20 },
            { typeof(BigAmber), 5}
        }, EDropModifier.REGULAR);

        public int flagId;
        private bool _hasBreak = false;
        private bool _isBeingLift = false;
        private bool _hasStartedBeingLift = false;

        private void Awake()
        {
            _spawnResetHandler = new SpawnResetHandler(transform);
            _flagHandler = new FlagHandler(flagId);
        }

        private void Update()
        {
            if (false && _hasBreak == false) Break();
        }

        private void OnDisable()
        {
            _spawnResetHandler.ResetToSpawnPosition();
            _isBeingLift = false;
            _hasStartedBeingLift = false;
            _hasBreak = false;
        }

        private void Break()
        {
            _hasBreak = true;
            _flagHandler.SetCurrentSceneFlag();
            // explode anim
            DropMonoHandler.PickAndSpawn(transform.position, EDropSpawnAnimation.HOP);
            Destroy(gameObject);
        }
    }
}
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
        private static readonly DropTable DropTable = new ( new Dictionary<Type, int>
        {
            { typeof(Heart), 20 },
            { typeof(SmallAmber), 20 }
        }, EDropModifier.REGULAR );
        private static readonly BreakableTable BreakableTable = new(new []
        {
            EDamageType.EXPLOSIVE,
        });

        private DropMonoHandler _dropMonoHandler;
        private SpawnResetHandler _spawnResetHandler;
        private FlagHandler _flagHandler;
        private LiftableMonoHandler _liftableMonoHandler;

        public int flagId;
        private bool _hasBreak = false;
        private bool _isBeingLift = false;
        private bool _hasStartedBeingLift = false;

        private void Awake()
        {
            _dropMonoHandler.Initialize(DropTable);
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
            _dropMonoHandler.PickAndSpawn(transform.position, EDropSpawnAnimation.HOP);
            Destroy(gameObject);
        }
    }
}
using System;
using System.Collections.Generic;
using Actors.Ables;
using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using Data.Definitions.CollectibleItems;
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
        private ISpawnResetable _spawnResetable;
        private FlagHandler _flagHandler;
        private LiftableMonoHandler _liftableMonoHandler;

        public int flagId;
        private bool _hasBreak = false;
        private bool _isBeingLift = false;
        private bool _hasStartedBeingLift = false;

        private void Awake()
        {
            _dropMonoHandler.Initialize(DropTable);
            _spawnResetable = new ISpawnResetable(transform);
            _flagHandler = new FlagHandler(flagId);
        }

        private void Update()
        {
            if (false && _hasBreak == false) Break();
        }

        private void OnDisable()
        {
            _spawnResetable.ResetToSpawnPosition();
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
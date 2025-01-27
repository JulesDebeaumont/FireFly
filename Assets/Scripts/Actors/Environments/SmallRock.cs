using System;
using System.Collections.Generic;
using Actors.Composites;
using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using Data.Definitions;
using Data.Definitions.CollectibleItems;
using UnityEngine;

namespace Actors.Environments
{
    /*
     * Liftable small rock
     */
    public class SmallRock : MonoBehaviour
    {
        private static readonly BreakableTable BreakableTable = new(new []
        {
            EDamageType.EXPLOSIVE,
        });
        

        public int flagId;
        private bool _hasBreak = false;
        private bool _isBeingLift = false;
        private bool _hasStartedBeingLift = false;

        private void Awake()
        {

        }

        private void Update()
        {
        }

        private void OnDisable()
        {
            _isBeingLift = false;
            _hasStartedBeingLift = false;
            _hasBreak = false;
        }
    }
}
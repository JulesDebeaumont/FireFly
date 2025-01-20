using System;
using System.Collections.Generic;
using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using Actors.Handlers;
using Actors.MonoHandlers;
using Manager;
using UnityEngine;

// https://www.youtube.com/watch?v=bUAOOLtak80
namespace Actors.Environments
{
    /*
     * Cuttable small grass
     */
    public class Grass : MonoBehaviour
    {
        
        private static readonly DropTable DropTable = new ( new Dictionary<Type, int>
        {
            { typeof(Heart), 20 },
            { typeof(SmallAmber), 20 }
        }, EDropModifier.REGULAR );
        private static readonly BreakableTable BreakableTable = new(new []
        {
            EDamageType.SWORD_REGULAR_SLASH,
            EDamageType.JUMPSLASH,
            EDamageType.EXPLOSIVE,
        });
        
        [SerializeField] private new Collider collider;
        
        private SpawnResetHandler _spawnResetHandler;
        private BreakableHandler _breakableHandler;
        private FlagHandler _flagHandler;
        private readonly DropMonoHandler _dropMonoHandler = new(DropTable);
        private readonly int _trackerPosition = Shader.PropertyToID("_trackerPosition");

        public int flagId;
        public Material material;
        private bool _hasBreak;

        private void Awake()
        {
            _spawnResetHandler = new SpawnResetHandler(transform);
            _flagHandler = new FlagHandler(flagId);
            _breakableHandler = new BreakableHandler(BreakableTable, collider, OnBreak, SetHasBreak);
            material.SetVector(_trackerPosition,
                PlayerManager.Instance.player.transform.position);
        }

        private void Update()
        {
        }


        private void OnDisable()
        {
            _spawnResetHandler.ResetToSpawnPosition();
        }

        private void OnBreak(EDamageType damageType)
        {
            _flagHandler.SetCurrentSceneFlag();
            // grass anim
            _dropMonoHandler.PickAndSpawn(transform.position, EDropSpawnAnimation.HOP);
            Destroy(gameObject);
        }

        private void SetHasBreak(bool hasBreak)
        {
            _hasBreak = hasBreak;
        }
    }
}
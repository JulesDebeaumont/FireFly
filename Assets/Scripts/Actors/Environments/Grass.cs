using System;
using System.Collections.Generic;
using Actors.Environments.CollectibleItems;
using Actors.Handlers;
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
        private SpawnResetHandler _spawnResetHandler;
        private FlagHandler _flagHandler;
        private static readonly DropCollectibleHandler DropCollectibleHandler = new( new Dictionary<Type, int>
        {
            { typeof(Heart), 20 },
            { typeof(SmallAmber), 20 }
        }, DropCollectibleHandler.EDropModifier.REGULAR);
        private static readonly int TrackerPosition = Shader.PropertyToID("_trackerPosition");

        public int flagId;
        public Material material;
        private bool _hasBreak;

        private void Awake()
        {
            _spawnResetHandler = new SpawnResetHandler(transform);
            _flagHandler = new FlagHandler(flagId);
            material.SetVector(TrackerPosition,
                PlayerManager.Instance.player.transform.position);
        }

        private void Update()
        {
            if (false && _hasBreak == false) Break();
        }


        private void OnDisable()
        {
            _spawnResetHandler.ResetToSpawnPosition();
            _hasBreak = false;
        }

        private void Break()
        {
            _hasBreak = true;
            _flagHandler.SetCurrentSceneFlag();
            // grass anim
            DropCollectibleHandler.PickAndSpawn(transform.position, DropCollectibleHandler.ESpawnAnimation.HOP);
            Destroy(gameObject);
        }
    }
}
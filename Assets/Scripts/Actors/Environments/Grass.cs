using System;
using System.Collections.Generic;
using Actors.Definitions;
using Actors.Environments.CollectibleItems;
using Data.Definitions;
using Data.Definitions.CollectibleItems;
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
        private static readonly DropTable DropTable = new ( new Dictionary<ECollectibleType, int>
        {
            { ECollectibleType.HEART, 20 },
            { ECollectibleType.SMALL_AMBER, 20 }
        }, EDropModifier.REGULAR );
        private static readonly BreakableTable BreakableTable = new(new []
        {
            EDamageType.SWORD_REGULAR_SLASH,
            EDamageType.JUMPSLASH,
            EDamageType.EXPLOSIVE,
        });
        
        [SerializeField] private new Collider collider;
        private readonly int _trackerPosition = Shader.PropertyToID("_trackerPosition");

        public int flagId;
        public Material material;
        private bool _hasBreak;

        private void Awake()
        {

            material.SetVector(_trackerPosition,
                PlayerManager.Instance.player.transform.position);
        }

        private void Update()
        {
        }


        private void OnDisable()
        {
            
        }

        private void OnBreak(EDamageType damageType)
        {

        }
        
    }
}
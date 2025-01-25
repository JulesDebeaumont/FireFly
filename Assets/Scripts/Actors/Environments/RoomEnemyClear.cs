using System.Collections.Generic;
using UnityEngine;

namespace Actors.Environments
{
    public class RoomEnemyClear : MonoBehaviour
    {
        private FlagHandler _flagHandler;
        public List<MonoBehaviour> enemyList = new();
        public int flagId;

        protected void Awake()
        {
            _flagHandler = new FlagHandler(flagId);
            if (_flagHandler.IsCurrentSceneFlagSet()) return;
            DestroyEnemies();
            Destroy(gameObject);
        }

        private void Update()
        {
            if (enemyList.Count != 0) return;
        }

        private void DestroyEnemies()
        {
            enemyList.Clear();
        }
    }
}
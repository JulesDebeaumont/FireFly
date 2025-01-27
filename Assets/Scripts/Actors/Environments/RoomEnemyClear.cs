using System.Collections.Generic;
using UnityEngine;

namespace Actors.Environments
{
    public class RoomEnemyClear : MonoBehaviour
    {
        public List<MonoBehaviour> enemyList = new();
        public int flagId;

        protected void Awake()
        {
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
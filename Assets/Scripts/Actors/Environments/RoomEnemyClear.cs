using System.Collections.Generic;
using Actors.Enemies;
using Manager;

namespace Actors.Environments
{
    public class RoomEnemyClear
    {
        public List<Enemy> enemyList = new();
        public int flagId;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            if (!PlayerManager.Instance.player.PlayerFlag.IsCurrentSceneFlagSet(flagId)) return;
            DestroyEnemies();
            Destroy(gameObject);
        }

        // Update is called once per frame
        private void Update()
        {
            if (enemyList.Count != 0) return;
            PlayerManager.Instance.player.PlayerFlag.SetCurrentSceneFlag(flagId);
            Destroy(gameObject);
        }

        private void DestroyEnemies()
        {
            enemyList.Clear();
        }
    }
}
#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomEnemyClear : Actor
{
    public List<Enemy?> EnemyList = new List<Enemy?> {};
    public int FlagId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (FlagId != 0 && PlayerManager.Instance.Player.PlayerFlag.IsCurrentSceneFlagSet(FlagId))
        {
            DestroyEnemies();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnemyList.Any(EnemyList => EnemyList != null))
        {
            PlayerManager.Instance.Player.PlayerFlag.SetCurrentSceneFlag(FlagId);
        }
    }

    private void DestroyEnemies()
    {
        foreach (Enemy? enemy in EnemyList)
        {
            if (enemy == null)
            {
                continue;
            }
            Destroy(enemy);
        }
    }

}

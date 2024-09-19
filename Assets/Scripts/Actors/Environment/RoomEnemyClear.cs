#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomEnemyClear : EnvironmentActor
{
    public List<Enemy?> EnemyList = new List<Enemy?> {};
    public int Flag = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Flag != 0 && SceneManager.Instance.IsFlagSet(Flag))
        {
            DestroyEnemies();
            Destroy(this.gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnemyList.Any(EnemyList => EnemyList != null))
        {
            SceneManager.Instance.SetFlag(Flag);
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

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
        if (this.Flag != 0 && SceneManager.Instance.IsFlagSet(this.Flag))
        {
            DestroyEnemies();
            Destroy(this.gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.EnemyList.Any(EnemyList => EnemyList != null))
        {
            SceneManager.Instance.SetFlag(this.Flag);
        }
    }

    private void DestroyEnemies()
    {
        foreach (Enemy? enemy in this.EnemyList)
        {
            if (enemy == null)
            {
                continue;
            }
            Destroy(enemy);
        }
    }

}

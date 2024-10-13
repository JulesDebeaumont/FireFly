using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    public FlagTable flagTable;

    void Awake()
    {
        flagTable = new FlagTable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    private List<int> _flags = new List<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public bool IsFlagSet(int flag)
    {
        return _flags.Contains(flag);
    }

    public void SetFlag(int flag)
    {
        if (_flags.Contains(flag))
        {
            return;
        }
        _flags.Add(flag);
    }
}

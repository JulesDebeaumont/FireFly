using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private List<int> Flags = new List<int>();
    public static SceneManager Instance { get; private set; }

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
        return this.Flags.Contains(flag);
    }

    public void SetFlag(int flag)
    {
        if (this.Flags.Contains(flag))
        {
            return;
        }
        this.Flags.Add(flag);
    }
}

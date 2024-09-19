using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : EnvironmentActor
{
    private readonly int _milliSecondTimer = 5000;
    public bool IsLit = false;
    public int Timer = 0;

    // TODO Model

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // timer going down

        // if fire is near
        if (FireIsClose())
        {
            IsLit = true;
            StartTimer();
        }
        if (IsLit == true && Timer == 0)
        {
            Unlit();
        }
    }

    private void StartTimer()
    {
        Timer = _milliSecondTimer;
    }

    private void Unlit()
    {
        IsLit = false;
        Timer = 0;
    }

    private bool FireIsClose()
    {
        return false;
    }
}

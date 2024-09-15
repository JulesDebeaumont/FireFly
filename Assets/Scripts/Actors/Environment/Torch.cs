using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : EnvironmentActor
{
    private readonly int MilliSecondTimer = 5000;
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
            this.IsLit = true;
            StartTimer();
        }
        if (this.IsLit == true && this.Timer == 0)
        {
            Unlit();
        }
    }

    private void StartTimer()
    {
        this.Timer = this.MilliSecondTimer;
    }

    private void Unlit()
    {
        this.IsLit = false;
        this.Timer = 0;
    }

    private bool FireIsClose()
    {
        return false;
    }
}

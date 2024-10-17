#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Sign : EnvironmentActor
{
    private TalkingActorUtils _talkingActorUtils;
    private bool _isBroken = false;

    void Awake()
    {
        _talkingActorUtils = new TalkingActorUtils();
    }

    void Update()
    {
        
    }

    public void Break()
    {

    }
}

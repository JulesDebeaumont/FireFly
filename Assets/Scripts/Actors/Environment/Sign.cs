#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Sign : EnvironmentActor
{
    public int DialogId = 1;
    private TalkingActorUtils _talkingActorUtils;
    private bool _isBroken = false;

    void Awake()
    {
        _talkingActorUtils = new TalkingActorUtils();
    }

    void Update()
    {
      if (_talkingActorUtils.PlayerInFrontOfTransform() && Input.GetKey(KeyCode.Y))
      {
        _talkingActorUtils.StartDialog(DialogId);
      }   
    }

    public void Break()
    {

    }
}

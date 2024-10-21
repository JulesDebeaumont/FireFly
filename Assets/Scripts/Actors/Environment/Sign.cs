#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Sign : Actor
{
  public int DialogId = 1;
  private TalkingActorUtils _talkingActorUtils;
  private bool _isBroken = false;

  void Awake()
  {
    base.Awake();
    _talkingActorUtils = new TalkingActorUtils();
  }

  void OnDisable()
  {
    base.OnDisable();
    _isBroken = false;
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

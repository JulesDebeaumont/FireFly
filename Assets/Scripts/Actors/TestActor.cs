using UnityEngine;

public class TestActor : Actor
{
  private bool _isRunning = false;

  protected override void OnDisable()
  {
    base.OnDisable();
    _isRunning = false;
  }

  void Update()
  {
    if (Input.GetKey(KeyCode.W) && _isRunning == false)
    {
      _isRunning = true;
      PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_IN_BLACK);
    }
    if (Input.GetKey(KeyCode.X) && _isRunning == false)
    {
      _isRunning = true;
      PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_OUT_BLACK);
    }
    if (Input.GetKey(KeyCode.C) && _isRunning == false)
    {
      _isRunning = true;
      PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_IN_WHITE);
    }
    if (Input.GetKey(KeyCode.V) && _isRunning == false)
    {
      _isRunning = true;
      PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_OUT_WHITE);
    }
    if (!PlayerManager.Instance.Player.PlayerCameraEffect.TransitionIsRunning)
    {
      _isRunning = false;
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawn : MonoBehaviour
{
  public GameObject GameObject;
  public int Id;
  public int SceneRoomId;
  public ESpawnPlayerAnimation SpawnPlayerAnimation;
  public ESpawnCameraTransition CameraTransition;
  private bool _hasBeenTriggered = false;

  void Start()
  {
    PlayerManager.Instance.Player.PlayerAction.SpawnAt(transform);
    switch (CameraTransition)
    {
      case ESpawnCameraTransition.FADE_OUT_BLACK:
        PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_OUT_BLACK);
        break;

      case ESpawnCameraTransition.FADE_OUT_WHITE:
        PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_OUT_WHITE);
        break;
    }
    _hasBeenTriggered = true;
  }

  public enum ESpawnPlayerAnimation
  {

  }

  public enum ESpawnCameraTransition
  {
    FADE_OUT_BLACK,
    FADE_OUT_WHITE
  }
}

using UnityEngine;

public class LoadingZone : MonoBehaviour
{
  [SerializeField] private BoxCollider _boxCollider;
  public int SceneDestinationId = 0;
  public int SceneDestionationSpawnId = 0;
  public ELoadingZoneCameraTransition CameraTransition = ELoadingZoneCameraTransition.FADE_IN_BLACK;
  private bool _hasBeenTriggered = false;
  private bool _isLoading = false;

  void Update()
  {
    var playerTouchLoadingZone = _boxCollider.bounds.Intersects(PlayerManager.Instance.Player.Collider.bounds);
    if (playerTouchLoadingZone && !_hasBeenTriggered && !_isLoading)
    {
      // TODO Lock Camera position, and maybe rotation too
      StartTransition();
      return;
    }
    if (_hasBeenTriggered && !PlayerManager.Instance.Player.PlayerCameraEffect.TransitionIsRunning && !_isLoading)
    {
      _isLoading = true;
      LoadNext();
    }
  }

  private void StartTransition()
  { 
    _hasBeenTriggered = true;
    switch (CameraTransition)
    {
      case ELoadingZoneCameraTransition.FADE_IN_BLACK:
        PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_IN_BLACK);
        break;

      case ELoadingZoneCameraTransition.FADE_IN_WHITE:
        PlayerManager.Instance.Player.PlayerCameraEffect.TriggerTransition(PlayerCameraEffect.ECameraTransition.FADE_IN_WHITE);
        break;
    }
  }

  private void LoadNext()
  {
    SceneManager.Instance.LoadScene(SceneDestinationId, SceneDestionationSpawnId);
  }

  public enum ELoadingZoneCameraTransition
  {
    FADE_IN_BLACK,
    FADE_IN_WHITE
  }
}

using UnityEngine;

public class LoadingZone : MonoBehaviour
{
  [SerializeField] private BoxCollider _boxCollider;
  public int SceneDestinationId = 1;
  public int SceneDestionationSpawnId = 1;
  public ELoadingZoneCameraTransition CameraTransition = ELoadingZoneCameraTransition.FADE_IN_BLACK;
  public bool KeepMusicOn = false;
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
      Debug.Log("LOADDDD");
      //LoadNext();
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
    var sceneToLoad = SceneTable.GetSceneNameById(SceneDestinationId);
    SceneManager.Instance.LoadScene(sceneToLoad);
  }

  public enum ELoadingZoneCameraTransition
  {
    FADE_IN_BLACK,
    FADE_IN_WHITE
  }
}

using UnityEngine;

public class LoadingZone : MonoBehaviour
{
  public int SceneDestinationId = 1;
  public int SceneDestionationSpawnId = 1;
  public ELoadingZoneCameraTransition CameraTransition = ELoadingZoneCameraTransition.FADE_IN_BLACK;
  public bool KeepMusicOn = false;
  private bool _hasBeenTriggered = false;

  void Update()
  {
    // TODO player is in box then load
    if (false)
    {
      StartTransition();
    }
  }

  private void StartTransition()
  { 
    if (PlayerManager.Instance.Player.PlayerCameraEffect.InBetweenRunning)
    {
      LoadNext();
      return;
    }

    if (PlayerManager.Instance.Player.PlayerCameraEffect.TransitionInRunning)
    {
      return;
    }
    _hasBeenTriggered = true;
    // TODO camera may stand where is it, but still focus on the character
    switch (CameraTransition)
    {
      case ELoadingZoneCameraTransition.FADE_IN_BLACK:
        PlayerManager.Instance.Player.PlayerCameraEffect.FadeInBlack();
        break;

      case ELoadingZoneCameraTransition.FADE_IN_WHITE:
        PlayerManager.Instance.Player.PlayerCameraEffect.FadeInWhite();
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

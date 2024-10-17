using UnityEngine;

public class PlayerCameraEffect : MonoBehaviour
{
  public Camera MainCamera;
  public bool TransitionInRunning = false;
  public bool TransitionOutRunning = false;
  public bool InBetweenRunning  = false;

  private readonly float _fadeInTransitionDefaultDuration = 2f;
  private readonly float _fadeOutTransitionDefaultDuration = 2f;

  public void FadeInBlack()
  {
    // spawn ui element, a black square, with alpha 0, then transition to all black
  }

  public void FadeInWhite()
  {

  }

  public void BlackScreen()
  {

  }

  public void WhiteScreen()
  {
    
  }

  public void FadeOutBlack()
  {

  }

  public void FadeOutWhite()
  {
    
  }

  public void UnderWater()
  {

  }

  public void Tremor()
  {

  }

  public void Blur()
  {

  }

  public void Clear()
  {

  }

}

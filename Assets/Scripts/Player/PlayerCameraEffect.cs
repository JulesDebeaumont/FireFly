using UnityEngine;

public class PlayerCameraEffect : MonoBehaviour
{
  public GameObject CameraEffectQuad;
  public bool TransitionIsRunning = false;

  private readonly float _fadeInTransitionDefaultDuration = 2f;
  private readonly float _fadeOutTransitionDefaultDuration = 2f;

  private Material _materialEffect;
  [SerializeField] private ECameraTransition _activeTransition = ECameraTransition.NONE;
  [SerializeField] private ECameraEffect _activeEffect = ECameraEffect.NONE;
  [SerializeField] private float _transitionRunningTimestamp;
  private Color _startColorTransition = new Color(0f, 0f, 0f, 0f);
  private Color _targetColorTransition = new Color(0f, 0f, 0f, 0f);

  void Start()
  {
    _materialEffect = CameraEffectQuad.GetComponent<Renderer>().material;
  }

  void Update()
  {
    switch (_activeTransition)
    {
      case ECameraTransition.NONE:
        break;

      case ECameraTransition.FADE_IN_BLACK:
        FadeInBlack();
        break;

      case ECameraTransition.FADE_IN_WHITE:
        FadeInWhite();
        break;

      case ECameraTransition.FADE_OUT_BLACK:
        FadeOutBlack();
        break;

      case ECameraTransition.FADE_OUT_WHITE:
        FadeOutWhite();
        break;
    }

    switch (_activeEffect)
    {
      case ECameraEffect.NONE:
        return;

      case ECameraEffect.UNDERWATER:
        UnderWater();
        break;

      case ECameraEffect.BLUR:
        Blur();
        break;

      case ECameraEffect.TREMOR:
        Tremor();
        break;
    }
  }

  public void TriggerEffect(ECameraEffect effect)
  {
    _activeEffect = effect;
  }

  public void TriggerTransition(ECameraTransition transition)
  {
    _activeTransition = transition;
  }

  private void FadeInBlack()
  {
    if (!TransitionIsRunning)
    {
      TransitionIsRunning = true;
      _targetColorTransition = new Color(0f, 0f, 0f, 1f);
      _startColorTransition = new Color(0f, 0f, 0f, 0f);
      var startColor = _targetColorTransition;
      startColor.a = _startColorTransition.a;
      _materialEffect.color = startColor;
      _transitionRunningTimestamp = Time.time;
      return;
    }
    var elapsed = Time.time - _transitionRunningTimestamp;
    var currentColor = _materialEffect.color;
    currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a, elapsed / _fadeInTransitionDefaultDuration);
    _materialEffect.color = currentColor;
    if (elapsed > _fadeInTransitionDefaultDuration)
    {
      TriggerTransition(ECameraTransition.NONE);
      TransitionIsRunning = false;
    }
  }

  private void FadeInWhite()
  {
    if (!TransitionIsRunning)
    {
      TransitionIsRunning = true;
      _targetColorTransition = new Color(1f, 1f, 1f, 1f);
      _startColorTransition = new Color(1f, 1f, 1f, 0f);
      var startColor = _targetColorTransition;
      startColor.a = _startColorTransition.a;
      _materialEffect.color = startColor;
      _transitionRunningTimestamp = Time.time;
      return;
    }
    var elapsed = Time.time - _transitionRunningTimestamp;
    var currentColor = _materialEffect.color;
    currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a, elapsed / _fadeInTransitionDefaultDuration);
    _materialEffect.color = currentColor;
    if (elapsed > _fadeInTransitionDefaultDuration)
    {
      TriggerTransition(ECameraTransition.NONE);
      TransitionIsRunning = false;
    }
  }

  private void FadeOutBlack()
  {
    if (!TransitionIsRunning)
    {
      TransitionIsRunning = true;
      _targetColorTransition = new Color(0f, 0f, 0f, 0f);
      _startColorTransition = new Color(0f, 0f, 0f, 1f);
      var startColor = _targetColorTransition;
      startColor.a = _startColorTransition.a;
      _materialEffect.color = startColor;
      _transitionRunningTimestamp = Time.time;
      return;
    }
    var elapsed = Time.time - _transitionRunningTimestamp;
    var currentColor = _materialEffect.color;
    currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a, elapsed / _fadeInTransitionDefaultDuration);
    _materialEffect.color = currentColor;
    if (elapsed > _fadeInTransitionDefaultDuration)
    {
      TriggerTransition(ECameraTransition.NONE);
      TransitionIsRunning = false;
    }
  }

  private void FadeOutWhite()
  {
    if (!TransitionIsRunning)
    {
      TransitionIsRunning = true;
      _targetColorTransition = new Color(1f, 1f, 1f, 0f);
      _startColorTransition = new Color(1f, 1f, 1f, 1f);
      var startColor = _targetColorTransition;
      startColor.a = _startColorTransition.a;
      _materialEffect.color = startColor;
      _transitionRunningTimestamp = Time.time;
      return;
    }
    var elapsed = Time.time - _transitionRunningTimestamp;
    var currentColor = _materialEffect.color;
    currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a, elapsed / _fadeInTransitionDefaultDuration);
    _materialEffect.color = currentColor;
    if (elapsed > _fadeInTransitionDefaultDuration)
    {
      TriggerTransition(ECameraTransition.NONE);
      TransitionIsRunning = false;
    }
  }

  private void UnderWater()
  {

  }

  private void Tremor()
  {
    // TODO not in effect but directly in Camera ?
  }

  private void Blur()
  {

  }

  public enum ECameraTransition
  {
    FADE_IN_BLACK,
    FADE_IN_WHITE,
    FADE_OUT_BLACK,
    FADE_OUT_WHITE,
    NONE
  }

  public enum ECameraEffect
  {
    BLUR,
    TREMOR,
    UNDERWATER,
    NONE
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
  public static DialogManager Instance { get; private set; }
  public GameObject UIDialogBox;
  public RectTransform UiDialogBoxRectTransform;
  public Image UIDialogPendingImage;
  public Image UIDialogBoxImage;
  public TMP_Text DialogText;
  public EDialogManagerState State = EDialogManagerState.SILENT;

  private readonly float _openingFadeDuration = 0.3f;
  private readonly float _openingFirstScaleDuration = 0.2f;
  private readonly float _openingSecondScaleDuration = 0.1f;
  private readonly float _transitionWaitDuration = 0.2f;
  private readonly float _closingFadeDuration = 0.05f;
  private readonly Vector3 _openingFirstScaleStart = new Vector3(0.7f, 0.7f, 1f);
  private readonly Vector3 _openingSecondScaleStart = new Vector3(1.05f, 1.05f, 1f);
  private readonly float _backgroundAlpha = 0.78f;
  private readonly Color _pendingColor = new Color(0.22f, 0.54f, 1f, 1f);

  [SerializeField] private float _animationTimestampFade;
  [SerializeField] private float _animationTimestampScale;
  [SerializeField] private float _writeTextTimestamp;
  [SerializeField] private bool _fadeRunning = false;
  [SerializeField] private bool _scaleRunning = false;
  [SerializeField] private bool _writeRunning = false;
  [SerializeField] private bool _transitionRunning = false;
  [SerializeField] private int _revealCurrentIndex = 0;
  [SerializeField] private int _sequenceIndex = 0;
  [SerializeField] private Color _targetColor = new Color(0f, 0f, 0f, 0f);
  [SerializeField] private Dialog? _currentDialog = null;
  private Dictionary<int, Dialog.EPieceOfTextColor> _coloredTextIndexes = new() { };
  private Dictionary<Dialog.EPieceOfTextAnimation, List<int[]>> _animatedTextIndexes = new() { };
  private Dialog.DialogSequence _currentSequence
  {
    get { return _currentDialog.Sequences[_sequenceIndex]; }
  }


  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      Instance = this;
    }
  }

  void FixedUpdate()
  {
    RunActionByState();
  }

  private void RunActionByState()
  {
    switch (State)
    {
      case EDialogManagerState.SILENT:
        return;

      case EDialogManagerState.OPENING1:
        OpenDialogFade();
        OpenDialogFirstScale();
        return;

      case EDialogManagerState.OPENING2:
        OpenDialogFade();
        OpenDialogSecondScale();
        return;

      case EDialogManagerState.WRITING:
        DisplayText();
        HandleText();
        return;

      case EDialogManagerState.PENDING:
        HandleText();
        Pending();
        return;

      case EDialogManagerState.TRANSITIONING:
        TransitioningToNextSequence();
        return;

      case EDialogManagerState.CLOSING:
        CloseDialog();
        return;
    }
  }

  private void SetupDialog(Dialog dialog)
  {
    UnsetDialog();
    _currentDialog = dialog;
    SetupSequence();
  }

  public void OpenDialog(Dialog dialog)
  {
    SetupDialog(dialog);
    State = EDialogManagerState.OPENING1;
  }

  private void OpenDialogFade()
  {
    if (!_fadeRunning)
    {
      _animationTimestampFade = Time.time;
      _targetColor = GetDialogBackground(_currentSequence.Background);
      var startColor = _targetColor;
      startColor.a = 0.3f;
      UIDialogBoxImage.color = startColor;
      _fadeRunning = true;
      return;
    }
    var elapsed = Time.time - _animationTimestampFade;
    var currentColor = UIDialogBoxImage.color;
    currentColor.a = Mathf.Lerp(0f, _targetColor.a, elapsed / _openingFadeDuration);
    UIDialogBoxImage.color = currentColor;
  }

  private void OpenDialogFirstScale()
  {
    if (!_scaleRunning)
    {
      _animationTimestampScale = Time.time;
      UiDialogBoxRectTransform.localScale = _openingFirstScaleStart;
      _scaleRunning = true;
      return;
    }
    var elapsed = Time.time - _animationTimestampScale;
    UiDialogBoxRectTransform.localScale = Vector3.Lerp(_openingFirstScaleStart, _openingSecondScaleStart, elapsed / _openingFirstScaleDuration);
    if (elapsed >= _openingFirstScaleDuration)
    {
      _scaleRunning = false;
      State = EDialogManagerState.OPENING2;
    }
  }

  private void OpenDialogSecondScale()
  {
    if (!_scaleRunning)
    {
      _animationTimestampScale = Time.time;
      _scaleRunning = true;
      return;
    }
    var elapsed = Time.time - _animationTimestampScale;
    UiDialogBoxRectTransform.localScale = Vector3.Lerp(_openingSecondScaleStart, new Vector3(1f, 1f, 1f), elapsed / _openingSecondScaleDuration);

    if (elapsed > _openingSecondScaleDuration)
    {
      _scaleRunning = false;
      _fadeRunning = false;
      State = EDialogManagerState.WRITING;
    }
  }

  private void Pending()
  {
    // TODO animation + diferent icon is closing or not
    var pendingColor = UIDialogPendingImage.color;
    pendingColor.a = 1f;
    UIDialogPendingImage.color = pendingColor;
    if (InputManager.Instance.ATap || InputManager.Instance.BTap || Input.GetKeyDown(KeyCode.T))
    {
      if (_currentDialog.Sequences.Length - 1 == _sequenceIndex)
      {
        State = EDialogManagerState.CLOSING;
      }
      else
      {
        _sequenceIndex += 1;
        HidePendingIcon();
        State = EDialogManagerState.TRANSITIONING;
      }
    }
  }

  private void HidePendingIcon()
  {
    var colorHide = UIDialogPendingImage.color;
    colorHide.a = 0f;
    UIDialogPendingImage.color = colorHide;
  }

  private void DisplayText()
  {
    if (_currentSequence.InstantText)
    {
      InstantText();
      return;
    }
    if (!_writeRunning)
    {
      _writeTextTimestamp = Time.time;
      _writeRunning = true;
      return;
    }

    var elapsed = Time.time - _writeTextTimestamp;
    if (elapsed >= _currentSequence.RevealSpeed)
    {
      if (_revealCurrentIndex < DialogText.text.Length)
      {
        ColorText();
        _revealCurrentIndex += 1;
      }
      else
      {
        if (_currentSequence.Choices.Length > 0)
        {
          DisplayChoices();
        }
        State = EDialogManagerState.PENDING;
      }
      _writeRunning = false;
    }
  }

  private void DisplayChoices()
  {
    // TODO
  }

  private void HandleText()
  {
    AnimateText();
    WatchForSkipText();
  }

  private void WatchForSkipText()
  {
    if (!_currentSequence.CanBeSkipped)
    {
      return;
    }
    if (InputManager.Instance.ATap || InputManager.Instance.BTap)
    {
      InstantText();
    }
  }

  private void InstantText()
  {
    for (int i = 0; i < DialogText.text.Length; i++)
    {
      ColorText();
      _revealCurrentIndex = i;
    }
    State = EDialogManagerState.PENDING;
    _writeRunning = false;
  }

  private void SetupTextAndIndexes()
  {
    _revealCurrentIndex = 0;
    var previousTextEndIndex = 0;
    for (int i = 0; i < _currentSequence.PiecesOfText.Length; i++)
    {
      var endIndex = previousTextEndIndex + _currentSequence.PiecesOfText[i].Text.Length;
      DialogText.text += _currentSequence.PiecesOfText[i].Text;

      // color
      for (int j = 0; j < _currentSequence.PiecesOfText[i].Text.Length; j++)
      {
        _coloredTextIndexes.Add(j + previousTextEndIndex, _currentSequence.PiecesOfText[i].Color);
      }

      // animation
      if (_currentSequence.PiecesOfText[i].Animation != Dialog.EPieceOfTextAnimation.NONE)
      {
        if (_animatedTextIndexes.TryGetValue(_currentSequence.PiecesOfText[i].Animation, out List<int[]> indexesAnimation))
        {
          indexesAnimation.Add(new int[] { previousTextEndIndex, endIndex });
        }
        else
        {
          _animatedTextIndexes.Add(_currentSequence.PiecesOfText[i].Animation, new List<int[]> { new int[] { previousTextEndIndex, endIndex } });
        }
      }
      previousTextEndIndex = endIndex;
    }

    DialogText.ForceMeshUpdate();
    var color32 = GetColor32ByPieceOfTextEnumColor(Dialog.EPieceOfTextColor.TRANSPARENT);
    var textInfo = DialogText.textInfo;
    for (int i = 0; i < DialogText.text.Length; i++)
    {
      var charInfo = textInfo.characterInfo[i];
      if (!charInfo.isVisible)
      {
        continue;
      }
      for (int j = 0; j < 4; j++) // 4 vertices
      {
        textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[charInfo.vertexIndex + j] = color32;
      }
    }
    DialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
  }

  private void ColorText()
  {

    if (_coloredTextIndexes.TryGetValue(_revealCurrentIndex, out Dialog.EPieceOfTextColor color))
    {
      var textInfo = DialogText.textInfo;
      var color32 = GetColor32ByPieceOfTextEnumColor(color);
      var charInfo = textInfo.characterInfo[_revealCurrentIndex];
      if (!charInfo.isVisible)
      {
        return;
      }
      for (int j = 0; j < 4; j++) // 4 vertices
      {
        textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[charInfo.vertexIndex + j] = color32;
      }
      DialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
  }

  private Color32 GetColor32ByPieceOfTextEnumColor(Dialog.EPieceOfTextColor color)
  {
    switch (color)
    {
      case Dialog.EPieceOfTextColor.WHITE:
        return new Color32(255, 255, 255, 255);

      case Dialog.EPieceOfTextColor.RED:
        return new Color32(240, 64, 64, 255);

      case Dialog.EPieceOfTextColor.YELLOW:
        return new Color32(232, 214, 66, 255);

      case Dialog.EPieceOfTextColor.GREEN:
        return new Color32(61, 158, 33, 255);

      case Dialog.EPieceOfTextColor.BLUE:
        return new Color32(33, 84, 186, 255);

      case Dialog.EPieceOfTextColor.BLACK:
        return new Color32(0, 0, 0, 255);

      case Dialog.EPieceOfTextColor.TRANSPARENT:
        return new Color32(0, 0, 0, 0);

      default:
        return new Color32(255, 255, 255, 255);
    }
  }

  private void AnimateText()
  {
    foreach (var entry in _animatedTextIndexes)
    {
      switch (entry.Key)
      {
        case Dialog.EPieceOfTextAnimation.CREEPY:
          CreepyAnimationText(entry.Value);
          break;

        case Dialog.EPieceOfTextAnimation.WOOBLE:
          WoobleAnimationText(entry.Value);
          break;

      }
    }
  }

  private void CreepyAnimationText(List<int[]> entries)
  {
    var textInfo = DialogText.textInfo;
    foreach (var arrayIndex in entries)
    {
      for (int i = arrayIndex[0]; i < arrayIndex[1]; i++)
      {
        var charInfo = textInfo.characterInfo[i];
        if (!charInfo.isVisible)
        {
          continue;
        }
        var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
        for (int j = 0; j < 4; j++) // 4 vertices
        {
          var origine = vertices[charInfo.vertexIndex + j];
          vertices[charInfo.vertexIndex + j] = origine + new Vector3(Mathf.Sin(Time.time * 50f + origine.y * 0.01f + i * 4f) * 0.7f, Mathf.Sin(Time.time * 50f + origine.y * 0.01f + i * 4f) * 0.7f, 0);
        }
      }

      for (int i = 0; i < textInfo.meshInfo.Length; i++)
      {
        var meshInfo = textInfo.meshInfo[i];
        meshInfo.mesh.vertices = meshInfo.vertices;
        DialogText.UpdateGeometry(meshInfo.mesh, i);
      }
    }
  }

  private void WoobleAnimationText(List<int[]> entries)
  {
    var textInfo = DialogText.textInfo;
    foreach (var arrayIndex in entries)
    {
      for (int i = arrayIndex[0]; i < arrayIndex[1]; i++)
      {
        var charInfo = textInfo.characterInfo[i];
        if (!charInfo.isVisible)
        {
          continue;
        }
        var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
        for (int j = 0; j < 4; j++) // 4 vertices
        {
          var origine = vertices[charInfo.vertexIndex + j];
          vertices[charInfo.vertexIndex + j] = origine + new Vector3(0, Mathf.Sin(Time.time * 5f + origine.x * 0.01f + 0.2f * i) * 0.5f, 0);
        }
      }

      for (int i = 0; i < textInfo.meshInfo.Length; i++)
      {
        var meshInfo = textInfo.meshInfo[i];
        meshInfo.mesh.vertices = meshInfo.vertices;
        DialogText.UpdateGeometry(meshInfo.mesh, i);
      }
    }
  }

  private void TransitioningToNextSequence()
  {
    if (!_transitionRunning)
    {
      UnsetSequence();
      SetupSequence();
      _animationTimestampFade = Time.time;
      _transitionRunning = true;
      return;
    }
    var elapsed = Time.time - _animationTimestampFade;
    if (elapsed > _transitionWaitDuration)
    {
      State = EDialogManagerState.WRITING;
      _transitionRunning = false;
    }
  }

  private void SetupSequence()
  {
    SetupTextAndIndexes();
  }

  private void UnsetSequence()
  {
    DialogText.text = string.Empty;
    _revealCurrentIndex = 0;
    _coloredTextIndexes.Clear();
    _animatedTextIndexes.Clear();
  }

  private void UnsetDialog()
  {
    UnsetSequence();
    _sequenceIndex = 0;
    _currentDialog = null;
  }

  private void CloseDialog()
  {
    if (!_fadeRunning)
    {
      _animationTimestampFade = Time.time;
      _targetColor = GetDialogBackground(Dialog.EDialogBackground.NONE);
      _fadeRunning = true;
      return;
    }
    var elapsed = Time.time - _animationTimestampFade;
    var closeColor = UIDialogPendingImage.color;
    closeColor.a = Mathf.Lerp(_backgroundAlpha, _targetColor.a, elapsed / _closingFadeDuration);
    UIDialogBoxImage.color = closeColor;
    if (elapsed > _closingFadeDuration)
    {
      HidePendingIcon();
      UnsetDialog();
      _fadeRunning = false;
      State = EDialogManagerState.SILENT;
    }
  }

  private Color GetDialogBackground(Dialog.EDialogBackground background)
  {
    switch (background)
    {
      case Dialog.EDialogBackground.STANDARD:
        return new Color(0f, 0f, 0f, _backgroundAlpha);

      case Dialog.EDialogBackground.NONE:
        return new Color(0f, 0f, 0f, 0f);

      case Dialog.EDialogBackground.WOOD:
        return new Color(0.26f, 0.05f, 0f, _backgroundAlpha);

      default:
        return new Color(0f, 0f, 0f, 0f);
    }
  }

  private void SetupDialogHeightByRowCount()
  {
    var nextSize = UiDialogBoxRectTransform.sizeDelta;
    nextSize.y = (_currentSequence.RowCount * 25f) + 50f;
    UiDialogBoxRectTransform.sizeDelta = nextSize;
  }

  public enum EDialogManagerState
  {
    SILENT,
    OPENING1,
    OPENING2,
    WRITING,
    TRANSITIONING,
    PENDING,
    CLOSING
  }
}

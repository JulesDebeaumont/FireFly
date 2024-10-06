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

    private readonly float _openingFadeDuration = 0.9f;
    private readonly float _openingFirstScaleDuration = 0.5f;
    private readonly float _openingSecondScaleDuration = 0.2f;
    private readonly float _openingThirdScaleDuration = 0.2f;
    private readonly float _closingFadeDuration = 0.2f;
    private readonly float _nextDialogWaitDuration = 0.2f;
    private readonly float _defaultTextSpeed = 0.03f;
    private readonly Vector3 _openingFirstScaleStart = new Vector3(0.5f, 0.5f, 1f);
    private readonly float _openingSecondScaleStart = 1.5f;
    private readonly float _backgroundAlpha = 0.78f;
    private readonly Color _pendingColor = new Color(0.22f, 0.54f, 1f, 1f);

    private float _animationTimestampFade = Time.time;
    private float _animationTimestampScale = Time.time;
    private bool _fadeRunning = false;
    private bool _scaleRunning = false;
    private int _writeCurrentIndex = 0;
    private string _flattenAllText = "";
    private Dictionary<Dialog.PieceOfText.EPieceOfTextColor, List<int[]>> _coloredTextIndexes = new Dictionary<Dialog.PieceOfText.EPieceOfTextColor, List<int[]>> { };
    private Dictionary<Dialog.PieceOfText.EPieceOfTextAnimation, List<int[]>> _animatedTextIndexes = new Dictionary<Dialog.PieceOfText.EPieceOfTextAnimation, List<int[]>> { };
    private Color _targetColor = new Color(0f, 0f, 0f, 0f);
    private Dialog? _currentDialog = null;
    private bool _aPress = false;
    private bool _bPress = false;
    private PlayerControl _playerControl;


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
        ReadInput();
    }

    void Start()
    {


    }

    void Update()
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
                WriteText();
                return;

            case EDialogManagerState.PENDING:
                WriteText();
                Pending();
                return;

            case EDialogManagerState.TRANSITIONING:
                Transitioning();
                return;

            case EDialogManagerState.CLOSING:
                CloseDialog();
                return;
        }
    }

    public void SetupDialog(Dialog dialog)
    {
        _currentDialog = dialog;
        SetupTextAndIndexes();
        State = EDialogManagerState.OPENING1;
    }

    private void OpenDialogFade()
    {
        if (!_fadeRunning)
        {
            _animationTimestampFade = Time.time;
            _targetColor = GetDialogBackground(_currentDialog.Background);
            var startColor = _targetColor;
            startColor.a = 0f;
            UIDialogBoxImage.color = startColor;
            _scaleRunning = true;
            return;
        }
        var elapsed = Time.time - _animationTimestampFade;
        var currentColor = UIDialogBoxImage.color;
        currentColor.a = Mathf.Lerp(0f, _targetColor.a, elapsed / _openingFadeDuration);
        UIDialogBoxImage.color = currentColor;
        if (elapsed > _openingFadeDuration)
        {
            _fadeRunning = false;
        }
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
        UiDialogBoxRectTransform.localScale = Mathf.Lerp(_openingFirstScaleStart, _openingSecondScaleStart, elapsed / _openingFirstScaleDuration);
        if (elapsed > _openingFirstScaleDuration)
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
            UIDialogBox.scale.x = _openingSecondScaleStart; // already set if everything's fine
            _scaleRunning = true;
            return;
        }
        var elapsed = Time.time - _animationTimestampScale;
        UIDialogBox.scale.x = Mathf.Lerp(_openingSecondScaleStart, 1.0f, elapsed / _openingSecondScaleDuration);
        if (elapsed > _openingSecondScaleDuration)
        {
            _scaleRunning = false;
            State = EDialogManagerState.WRITING;
        }
    }

    private void Pending()
    {
        // TODO
        var color = UIDialogPendingImage.color;
        color.a = 1f;
        UIDialogPendingImage.color = color;
        if (!_aPress && !_bPress)
        {
            return;
        }
        if (_currentDialog.NextDialog == null)
        {
            State = EDialogManagerState.CLOSING;
        }
        else
        {
            var colorHide = UIDialogPendingImage.color;
            colorHide.a = 0f;
            UIDialogPendingImage.color = colorHide;
            State = EDialogManagerState.TRANSITIONING;
        }
    }

    private void WriteText()
    {
        DisplayText();
        ColorText();
        AnimateText();
        if (_writeCurrentIndex == _flattenAllText.Length)
        {
            State = EDialogManagerState.PENDING;
        }
    }

    private void SetupTextAndIndexes()
    {
        _writeCurrentIndex = 0;
        var previousTextEndIndex = 0;
        for (var i = 0; i <= _currentDialog.PiecesOfText.Count; i++)
        {
            var endIndex = previousTextEndIndex + _currentDialog.PiecesOfText[i].Text.Length;
            _flattenAllText += _currentDialog.PiecesOfText[i].Text;

            // color
            if (_coloredTextIndexes.TryGetValue(_currentDialog.PiecesOfText[i].Color, out List<int[]> indexesColor))
            {
                indexesColor.Add(new int[] { previousTextEndIndex, endIndex });
            }
            else
            {
                _animatedTextIndexes.Add(_currentDialog.PiecesOfText[i].Color, new List<int[]> { new int[] { previousTextEndIndex, endIndex } });
            }

            // animation
            if (_currentDialog.PiecesOfText[i].Animation != Dialog.PieceOfText.EPieceOfTextAnimation.NONE)
            {
                if (_animatedTextIndexes.TryGetValue(_currentDialog.PiecesOfText[i].Animation, out List<int[]> indexesAnimation))
                {
                    indexesAnimation.Add(new int[] { previousTextEndIndex, endIndex });
                }
                else
                {
                    _animatedTextIndexes.Add(_currentDialog.PiecesOfText[i].Animation, new List<int[]> { new int[] { previousTextEndIndex, endIndex } });
                }
            }
            previousTextEndIndex = endIndex;
        }
        var textInfo = DialogText.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            textInfo.characterInfo[i].isVisible = false;
        }
    }

    private void DisplayText()
    {
        var textInfo = DialogText.textInfo;
        if (_writeCurrentIndex > textInfo.characterCount)
        {
            return;
        }
        textInfo.characterInfo[_writeCurrentIndex].isVisible = false;
        _writeCurrentIndex += 1;
    }

    private void ColorText()
    {
        foreach (var entry in _coloredTextIndexes)
        {
            var color = GetColorByPieceOfTextEnumColor(entry.Key);

            DialogText.ForceMeshUpdate();
            var textInfo = DialogText.textInfo;

            foreach (var arrayIndex in entry.Value)
            {
                for (int i = arrayIndex[0]; i <= arrayIndex[1]; i++)
                {
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible)
                    {
                        continue;
                    }
                    charInfo.colors32[i] = color;
                }
            }
        }
    }

    private Color GetColorByPieceOfTextEnumColor(Dialog.PieceOfText.EPieceOfTextColor color)
    {
        switch (color)
        {
            case Dialog.PieceOfText.EPieceOfTextColor.WHITE:
                return new Color(1f, 1f, 1f, 1f);

            case Dialog.PieceOfText.EPieceOfTextColor.RED:
                return new Color(0.94f, 0.25f, 0.25f, 1f);

            case Dialog.PieceOfText.EPieceOfTextColor.YELLOW:
                return new Color(0.91f, 0.84f, 0.26f, 1f);

            case Dialog.PieceOfText.EPieceOfTextColor.GREEN:
                return new Color(0.24f, 0.62f, 0.13f, 1f);

            case Dialog.PieceOfText.EPieceOfTextColor.BLUE:
                return new Color(0.13f, 0.33f, 0.73f, 1f);

            case Dialog.PieceOfText.EPieceOfTextColor.BLACK:
                return new Color(0.0f, 0.0f, 0.0f, 1f);

            default:
                return new Color(1f, 1f, 1f, 1f);
        }
    }

    private void AnimateText()
    {
        foreach (var entry in _animatedTextIndexes)
        {
            switch (entry.Key)
            {
                case Dialog.PieceOfText.EPieceOfTextAnimation.CREEPY:
                    CreepyAnimationText(entry.Value);
                    break;

                case Dialog.PieceOfText.EPieceOfTextAnimation.SUSPENSE:
                    SuspenseAnimationText(entry.Value);
                    break;

                case Dialog.PieceOfText.EPieceOfTextAnimation.WOOBLE:
                    WoobleAnimationText(entry.Value);
                    break;

            }
        }
    }

    private void CreepyAnimationText(List<int[]> entries)
    {

    }

    private void SuspenseAnimationText(List<int[]> entries)
    {

    }

    private void WoobleAnimationText(List<int[]> entries)
    {
        DialogText.ForceMeshUpdate();
        var textInfo = DialogText.textInfo;

        foreach (var arrayIndex in entries)
        {
            for (int i = arrayIndex[0]; i <= arrayIndex[1]; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                {
                    continue;
                }
                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                for (int j = 0; j < 4; j++) // moving the 4 vertices at once
                {
                    var origine = vertices[charInfo.vertexIndex + j];
                    vertices[charInfo.vertexIndex + j] = origine + new Vector3(0, Mathf.Sin(Time.time * 2.0f + origine.x * 0.01f) * 10.f, 0);
                }
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                DialogText.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }

    private void Transitioning()
    {

    }

    private void UnsetTextAndIndexes()
    {
        _writeCurrentIndex = 0;
        _flattenAllText = string.Empty;
        _coloredTextIndexes.Clear();
        _animatedTextIndexes.Clear();
    }

    private void CloseDialog()
    {
        if (!_fadeRunning)
        {
            _animationTimestampFade = Time.time;
            _targetColor = GetDialogBackground(Dialog.EDialogBackground.NONE);
            _scaleRunning = true;
            return;
        }
        var elapsed = Time.time - _animationTimestampFade;
        UIDialogBoxImage.color.a = Mathf.Lerp(_backgroundAlpha, _targetColor.a, elapsed / _closingFadeDuration);
        if (elapsed > _closingFadeDuration)
        {
            UnsetTextAndIndexes();
            DialogText.text = "";
            _currentDialog = null;
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

    private void SetupDialogHeightByRowCount(int rowCount)
    {
        UIDialogBoxImage.preferredHeight = (rowCount * 25f) + 50f;
    }

    private void ReadInput()
    {
        _playerControl = new PlayerControl();

        _playerControl.Gameplay.A.performed += context => _aPress = true;
        _playerControl.Gameplay.A.canceled += context => _aPress = false;
        _playerControl.Gameplay.B.performed += context => _bPress = true;
        _playerControl.Gameplay.B.canceled += context => _bPress = false;
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

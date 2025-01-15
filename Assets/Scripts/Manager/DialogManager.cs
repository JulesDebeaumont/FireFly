using System;
using System.Collections.Generic;
using Data.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class DialogManager : MonoBehaviour
    {
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

        public GameObject uiDialogBox;
        public RectTransform uiDialogBoxRectTransform;
        public Image uiDialogPendingImage;
        public Image uiDialogBoxImage;
        public TMP_Text dialogText;
        public EDialogManagerState state = EDialogManagerState.SILENT;

        [SerializeField] private float animationTimestampFade;
        [SerializeField] private float animationTimestampScale;
        [SerializeField] private float writeTextTimestamp;
        [SerializeField] private bool fadeRunning;
        [SerializeField] private bool scaleRunning;
        [SerializeField] private bool writeRunning;
        [SerializeField] private bool transitionRunning;
        [SerializeField] private int revealCurrentIndex;
        [SerializeField] private int sequenceIndex;
        [SerializeField] private Color targetColor = new(0f, 0f, 0f, 0f);
        private readonly float _backgroundAlpha = 0.78f;
        private readonly float _closingFadeDuration = 0.05f;

        private readonly float _openingFadeDuration = 0.3f;
        private readonly float _openingFirstScaleDuration = 0.2f;
        private readonly Vector3 _openingFirstScaleStart = new(0.7f, 0.7f, 1f);
        private readonly float _openingSecondScaleDuration = 0.1f;
        private readonly Vector3 _openingSecondScaleStart = new(1.05f, 1.05f, 1f);
        private readonly Color _pendingColor = new(0.22f, 0.54f, 1f, 1f);
        private readonly float _transitionWaitDuration = 0.2f;
        private readonly Dictionary<Dialog.EPieceOfTextAnimation, List<int[]>> _animatedTextIndexes = new();
        private readonly Dictionary<int, Dialog.EPieceOfTextColor> _coloredTextIndexes = new();
        private Dialog? _currentDialog;

        private Dialog.DialogSequence CurrentSequence => _currentDialog.Sequences[sequenceIndex];

        private void FixedUpdate()
        {
            RunActionByState();
        }

        private void RunActionByState()
        {
            switch (state)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupDialog(Dialog dialog)
        {
            _currentDialog = dialog;
            SetupSequence();
        }

        public void OpenDialog(Dialog dialog)
        {
            UnsetDialog();
            SetupDialog(dialog);
            state = EDialogManagerState.OPENING1;
        }

        private void OpenDialogFade()
        {
            if (!fadeRunning)
            {
                animationTimestampFade = Time.time;
                targetColor = GetDialogBackground(CurrentSequence.Background);
                var startColor = targetColor;
                startColor.a = 0.3f;
                uiDialogBoxImage.color = startColor;
                fadeRunning = true;
                return;
            }

            var elapsed = Time.time - animationTimestampFade;
            var currentColor = uiDialogBoxImage.color;
            currentColor.a = Mathf.Lerp(0f, targetColor.a, elapsed / _openingFadeDuration);
            uiDialogBoxImage.color = currentColor;
        }

        private void OpenDialogFirstScale()
        {
            if (!scaleRunning)
            {
                animationTimestampScale = Time.time;
                uiDialogBoxRectTransform.localScale = _openingFirstScaleStart;
                scaleRunning = true;
                return;
            }

            var elapsed = Time.time - animationTimestampScale;
            uiDialogBoxRectTransform.localScale = Vector3.Lerp(_openingFirstScaleStart, _openingSecondScaleStart,
                elapsed / _openingFirstScaleDuration);
            if (!(elapsed >= _openingFirstScaleDuration)) return;
            scaleRunning = false;
            state = EDialogManagerState.OPENING2;
        }

        private void OpenDialogSecondScale()
        {
            if (!scaleRunning)
            {
                animationTimestampScale = Time.time;
                scaleRunning = true;
                return;
            }

            var elapsed = Time.time - animationTimestampScale;
            uiDialogBoxRectTransform.localScale = Vector3.Lerp(_openingSecondScaleStart, new Vector3(1f, 1f, 1f),
                elapsed / _openingSecondScaleDuration);

            if (!(elapsed > _openingSecondScaleDuration)) return;
            scaleRunning = false;
            fadeRunning = false;
            state = EDialogManagerState.WRITING;
        }

        private void Pending()
        {
            // TODO animation + diferent icon is closing or not
            var pendingColor = uiDialogPendingImage.color;
            pendingColor.a = 1f;
            uiDialogPendingImage.color = pendingColor;
            if (!InputManager.Instance.ATap && !InputManager.Instance.BTap && !Input.GetKeyDown(KeyCode.T)) return;
            if (_currentDialog.Sequences.Length - 1 == sequenceIndex)
            {
                state = EDialogManagerState.CLOSING;
            }
            else
            {
                sequenceIndex += 1;
                HidePendingIcon();
                state = EDialogManagerState.TRANSITIONING;
            }
        }

        private void HidePendingIcon()
        {
            var colorHide = uiDialogPendingImage.color;
            colorHide.a = 0f;
            uiDialogPendingImage.color = colorHide;
        }

        private void DisplayText()
        {
            if (CurrentSequence.InstantText)
            {
                InstantText();
                return;
            }

            if (!writeRunning)
            {
                writeTextTimestamp = Time.time;
                writeRunning = true;
                return;
            }

            var elapsed = Time.time - writeTextTimestamp;
            if (!(elapsed >= CurrentSequence.RevealSpeed)) return;
            if (revealCurrentIndex < dialogText.text.Length)
            {
                ColorText();
                revealCurrentIndex += 1;
            }
            else
            {
                if (CurrentSequence.Choices.Length > 0) DisplayChoices();
                state = EDialogManagerState.PENDING;
            }

            writeRunning = false;
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
            if (!CurrentSequence.CanBeSkipped) return;
            if (InputManager.Instance.ATap || InputManager.Instance.BTap) InstantText();
        }

        private void InstantText()
        {
            for (var i = 0; i < dialogText.text.Length; i++)
            {
                ColorText();
                revealCurrentIndex = i;
            }

            state = EDialogManagerState.PENDING;
            writeRunning = false;
        }

        private void SetupTextAndIndexes()
        {
            revealCurrentIndex = 0;
            var previousTextEndIndex = 0;
            foreach (var t in CurrentSequence.PiecesOfText)
            {
                var endIndex = previousTextEndIndex + t.Text.Length;
                dialogText.text += t.Text;

                // color
                for (var j = 0; j < t.Text.Length; j++)
                    _coloredTextIndexes.Add(j + previousTextEndIndex, t.Color);

                // animation
                if (t.Animation != Dialog.EPieceOfTextAnimation.NONE)
                {
                    if (_animatedTextIndexes.TryGetValue(t.Animation,
                            out var indexesAnimation))
                        indexesAnimation.Add(new[] { previousTextEndIndex, endIndex });
                    else
                        _animatedTextIndexes.Add(t.Animation,
                            new List<int[]> { new[] { previousTextEndIndex, endIndex } });
                }

                previousTextEndIndex = endIndex;
            }

            dialogText.ForceMeshUpdate();
            var color32 = GetColor32ByPieceOfTextEnumColor(Dialog.EPieceOfTextColor.TRANSPARENT);
            var textInfo = dialogText.textInfo;
            for (var i = 0; i < dialogText.text.Length; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;
                for (var j = 0; j < 4; j++) // 4 vertices
                    textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[charInfo.vertexIndex + j] = color32;
            }

            dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        private void ColorText()
        {
            if (!_coloredTextIndexes.TryGetValue(revealCurrentIndex, out var color)) return;
            var textInfo = dialogText.textInfo;
            var color32 = GetColor32ByPieceOfTextEnumColor(color);
            var charInfo = textInfo.characterInfo[revealCurrentIndex];
            if (!charInfo.isVisible) return;
            for (var j = 0; j < 4; j++) // 4 vertices
                textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[charInfo.vertexIndex + j] = color32;
            dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        private static Color32 GetColor32ByPieceOfTextEnumColor(Dialog.EPieceOfTextColor color)
        {
            return color switch
            {
                Dialog.EPieceOfTextColor.WHITE => new Color32(255, 255, 255, 255),
                Dialog.EPieceOfTextColor.RED => new Color32(240, 64, 64, 255),
                Dialog.EPieceOfTextColor.YELLOW => new Color32(232, 214, 66, 255),
                Dialog.EPieceOfTextColor.GREEN => new Color32(61, 158, 33, 255),
                Dialog.EPieceOfTextColor.BLUE => new Color32(33, 84, 186, 255),
                Dialog.EPieceOfTextColor.BLACK => new Color32(0, 0, 0, 255),
                Dialog.EPieceOfTextColor.TRANSPARENT => new Color32(0, 0, 0, 0),
                _ => new Color32(255, 255, 255, 255)
            };
        }

        private void AnimateText()
        {
            foreach (var entry in _animatedTextIndexes)
                switch (entry.Key)
                {
                    case Dialog.EPieceOfTextAnimation.CREEPY:
                        CreepyAnimationText(entry.Value);
                        break;

                    case Dialog.EPieceOfTextAnimation.WOOBLE:
                        WoobleAnimationText(entry.Value);
                        break;
                    case Dialog.EPieceOfTextAnimation.NONE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        private void CreepyAnimationText(List<int[]> entries)
        {
            var textInfo = dialogText.textInfo;
            foreach (var arrayIndex in entries)
            {
                for (var i = arrayIndex[0]; i < arrayIndex[1]; i++)
                {
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;
                    var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    for (var j = 0; j < 4; j++) // 4 vertices
                    {
                        var origine = vertices[charInfo.vertexIndex + j];
                        vertices[charInfo.vertexIndex + j] = origine + new Vector3(
                            Mathf.Sin(Time.time * 50f + origine.y * 0.01f + i * 4f) * 0.7f,
                            Mathf.Sin(Time.time * 50f + origine.y * 0.01f + i * 4f) * 0.7f, 0);
                    }
                }

                for (var i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    var meshInfo = textInfo.meshInfo[i];
                    meshInfo.mesh.vertices = meshInfo.vertices;
                    dialogText.UpdateGeometry(meshInfo.mesh, i);
                }
            }
        }

        private void WoobleAnimationText(List<int[]> entries)
        {
            var textInfo = dialogText.textInfo;
            foreach (var arrayIndex in entries)
            {
                for (var i = arrayIndex[0]; i < arrayIndex[1]; i++)
                {
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;
                    var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    for (var j = 0; j < 4; j++) // 4 vertices
                    {
                        var origine = vertices[charInfo.vertexIndex + j];
                        vertices[charInfo.vertexIndex + j] = origine + new Vector3(0,
                            Mathf.Sin(Time.time * 5f + origine.x * 0.01f + 0.2f * i) * 0.5f, 0);
                    }
                }

                for (var i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    var meshInfo = textInfo.meshInfo[i];
                    meshInfo.mesh.vertices = meshInfo.vertices;
                    dialogText.UpdateGeometry(meshInfo.mesh, i);
                }
            }
        }

        private void TransitioningToNextSequence()
        {
            if (!transitionRunning)
            {
                UnsetSequence();
                SetupSequence();
                animationTimestampFade = Time.time;
                transitionRunning = true;
                return;
            }

            var elapsed = Time.time - animationTimestampFade;
            if (!(elapsed > _transitionWaitDuration)) return;
            state = EDialogManagerState.WRITING;
            transitionRunning = false;
        }

        private void SetupSequence()
        {
            SetupTextAndIndexes();
        }

        private void UnsetSequence()
        {
            dialogText.text = string.Empty;
            revealCurrentIndex = 0;
            _coloredTextIndexes.Clear();
            _animatedTextIndexes.Clear();
        }

        private void UnsetDialog()
        {
            UnsetSequence();
            sequenceIndex = 0;
            _currentDialog = null;
        }

        private void CloseDialog()
        {
            if (!fadeRunning)
            {
                animationTimestampFade = Time.time;
                targetColor = GetDialogBackground(Dialog.EDialogBackground.NONE);
                fadeRunning = true;
                return;
            }

            var elapsed = Time.time - animationTimestampFade;
            var closeColor = uiDialogPendingImage.color;
            closeColor.a = Mathf.Lerp(_backgroundAlpha, targetColor.a, elapsed / _closingFadeDuration);
            uiDialogBoxImage.color = closeColor;
            if (!(elapsed > _closingFadeDuration)) return;
            HidePendingIcon();
            UnsetDialog();
            fadeRunning = false;
            state = EDialogManagerState.SILENT;
        }

        private Color GetDialogBackground(Dialog.EDialogBackground background)
        {
            return background switch
            {
                Dialog.EDialogBackground.STANDARD => new Color(0f, 0f, 0f, _backgroundAlpha),
                Dialog.EDialogBackground.NONE => new Color(0f, 0f, 0f, 0f),
                Dialog.EDialogBackground.WOOD => new Color(0.26f, 0.05f, 0f, _backgroundAlpha),
                _ => new Color(0f, 0f, 0f, 0f)
            };
        }

        private void SetupDialogHeightByRowCount()
        {
            var nextSize = uiDialogBoxRectTransform.sizeDelta;
            nextSize.y = CurrentSequence.RowCount * 25f + 50f;
            uiDialogBoxRectTransform.sizeDelta = nextSize;
        }
    }
}
using System;
using UnityEngine;

namespace Player
{
    public class PlayerCameraEffect : MonoBehaviour
    {
        public enum ECameraEffect
        {
            BLUR,
            TREMOR,
            UNDERWATER,
            NONE
        }

        public enum ECameraTransition
        {
            FADE_IN_BLACK,
            FADE_IN_WHITE,
            FADE_OUT_BLACK,
            FADE_OUT_WHITE,
            NONE
        }

        public GameObject cameraEffectQuad;
        public bool transitionIsRunning;
        [SerializeField] private ECameraTransition activeTransition = ECameraTransition.NONE;
        [SerializeField] private ECameraEffect activeEffect = ECameraEffect.NONE;
        [SerializeField] private float transitionRunningTimestamp;

        private const float FadeInTransitionDefaultDuration = 2f;
        private const float FadeOutTransitionDefaultDuration = 2f;

        private Material _materialEffect;
        private Color _startColorTransition = new(0f, 0f, 0f, 0f);
        private Color _targetColorTransition = new(0f, 0f, 0f, 0f);

        private void Start()
        {
            _materialEffect = cameraEffectQuad.GetComponent<Renderer>().material;
        }

        private void Update()
        {
            switch (activeTransition)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (activeEffect)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TriggerEffect(ECameraEffect effect)
        {
            activeEffect = effect;
        }

        public void TriggerTransition(ECameraTransition transition)
        {
            activeTransition = transition;
        }

        private void FadeInBlack()
        {
            if (!transitionIsRunning)
            {
                transitionIsRunning = true;
                _targetColorTransition = new Color(0f, 0f, 0f, 1f);
                _startColorTransition = new Color(0f, 0f, 0f, 0f);
                var startColor = _targetColorTransition;
                startColor.a = _startColorTransition.a;
                _materialEffect.color = startColor;
                transitionRunningTimestamp = Time.time;
                return;
            }

            var elapsed = Time.time - transitionRunningTimestamp;
            var currentColor = _materialEffect.color;
            currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a,
                elapsed / FadeInTransitionDefaultDuration);
            _materialEffect.color = currentColor;
            if (!(elapsed > FadeInTransitionDefaultDuration)) return;
            TriggerTransition(ECameraTransition.NONE);
            transitionIsRunning = false;
        }

        private void FadeInWhite()
        {
            if (!transitionIsRunning)
            {
                transitionIsRunning = true;
                _targetColorTransition = new Color(1f, 1f, 1f, 1f);
                _startColorTransition = new Color(1f, 1f, 1f, 0f);
                var startColor = _targetColorTransition;
                startColor.a = _startColorTransition.a;
                _materialEffect.color = startColor;
                transitionRunningTimestamp = Time.time;
                return;
            }

            var elapsed = Time.time - transitionRunningTimestamp;
            var currentColor = _materialEffect.color;
            currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a,
                elapsed / FadeInTransitionDefaultDuration);
            _materialEffect.color = currentColor;
            if (!(elapsed > FadeInTransitionDefaultDuration)) return;
            TriggerTransition(ECameraTransition.NONE);
            transitionIsRunning = false;
        }

        private void FadeOutBlack()
        {
            if (!transitionIsRunning)
            {
                transitionIsRunning = true;
                _targetColorTransition = new Color(0f, 0f, 0f, 0f);
                _startColorTransition = new Color(0f, 0f, 0f, 1f);
                var startColor = _targetColorTransition;
                startColor.a = _startColorTransition.a;
                _materialEffect.color = startColor;
                transitionRunningTimestamp = Time.time;
                return;
            }

            var elapsed = Time.time - transitionRunningTimestamp;
            var currentColor = _materialEffect.color;
            currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a,
                elapsed / FadeOutTransitionDefaultDuration);
            _materialEffect.color = currentColor;
            if (!(elapsed > FadeOutTransitionDefaultDuration)) return;
            TriggerTransition(ECameraTransition.NONE);
            transitionIsRunning = false;
        }

        private void FadeOutWhite()
        {
            if (!transitionIsRunning)
            {
                transitionIsRunning = true;
                _targetColorTransition = new Color(1f, 1f, 1f, 0f);
                _startColorTransition = new Color(1f, 1f, 1f, 1f);
                var startColor = _targetColorTransition;
                startColor.a = _startColorTransition.a;
                _materialEffect.color = startColor;
                transitionRunningTimestamp = Time.time;
                return;
            }

            var elapsed = Time.time - transitionRunningTimestamp;
            var currentColor = _materialEffect.color;
            currentColor.a = Mathf.Lerp(_startColorTransition.a, _targetColorTransition.a,
                elapsed / FadeOutTransitionDefaultDuration);
            _materialEffect.color = currentColor;
            if (!(elapsed > FadeOutTransitionDefaultDuration)) return;
            TriggerTransition(ECameraTransition.NONE);
            transitionIsRunning = false;
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
    }
}
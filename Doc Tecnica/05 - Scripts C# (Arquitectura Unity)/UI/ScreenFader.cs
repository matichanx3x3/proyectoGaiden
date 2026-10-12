using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.UI {
    /// <summary>
    /// Control de fundido a negro (Fade In / Fade Out) en pantalla completa para transiciones de nivel y cinemáticas.
    /// Auto-instanciable bajo demanda y compatible con DOTween en tiempo no escalado.
    /// </summary>
    public class ScreenFader : MonoBehaviour {
        private static ScreenFader _instance;
        public static ScreenFader Instance {
            get {
                if (_instance == null) {
                    var existing = FindFirstObjectByType<ScreenFader>();
                    if (existing != null) {
                        _instance = existing;
                    } else {
                        var go = new GameObject("[Systems_ScreenFader]");
                        _instance = go.AddComponent<ScreenFader>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        private Canvas _canvas;
        private Image _fadeImage;

        private void Awake() {
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            } else if (_instance != this) {
                Destroy(gameObject);
                return;
            }

            EnsureUI();
        }

        private void EnsureUI() {
            if (_fadeImage != null) return;

            var canvasGo = new GameObject("FaderCanvas", typeof(Canvas), typeof(CanvasScaler));
            canvasGo.transform.SetParent(transform, false);
            _canvas = canvasGo.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 999; // Capa superior absoluta

            var imgGo = new GameObject("BlackOverlay", typeof(RectTransform), typeof(Image));
            imgGo.transform.SetParent(canvasGo.transform, false);
            var rt = imgGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;

            _fadeImage = imgGo.GetComponent<Image>();
            _fadeImage.color = new Color(0f, 0f, 0f, 0f);
            _fadeImage.raycastTarget = false;
        }

        public void FadeOut(float duration, Action onComplete = null) {
            EnsureUI();
            _fadeImage.DOKill();
            _fadeImage.raycastTarget = true;
            _fadeImage.DOFade(1f, duration).SetUpdate(true).OnComplete(() => {
                onComplete?.Invoke();
            });
        }

        public void FadeIn(float duration, Action onComplete = null) {
            EnsureUI();
            _fadeImage.DOKill();
            _fadeImage.DOFade(0f, duration).SetUpdate(true).OnComplete(() => {
                _fadeImage.raycastTarget = false;
                onComplete?.Invoke();
            });
        }
    }
}

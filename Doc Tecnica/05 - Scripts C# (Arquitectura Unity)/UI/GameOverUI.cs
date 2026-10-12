using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Game.Core;

namespace Game.UI {
    /// <summary>
    /// Pantalla de Game Over ("YOU ARE DEAD") cuando todos los miembros del grupo caen en combate.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class GameOverUI : MonoBehaviour {
        [SerializeField] private GameObject _gameOverPanel;

        private void Awake() {
            EnsureUIHierarchy();
        }

        private void OnEnable() {
            GameManager.OnAfterStateChanged += HandleStateChanged;
        }

        private void OnDisable() {
            GameManager.OnAfterStateChanged -= HandleStateChanged;
        }

        private void Start() {
            if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        }

        private void HandleStateChanged(GameState newState) {
            bool isGameOver = (newState == GameState.GameOver);
            if (_gameOverPanel != null) {
                _gameOverPanel.SetActive(isGameOver);
            }
        }

        public void RestartGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void EnsureUIHierarchy() {
            if (_gameOverPanel != null) return;

            var existing = transform.Find("GameOverPanel");
            if (existing != null) {
                _gameOverPanel = existing.gameObject;
                return;
            }

            var panelGo = new GameObject("GameOverPanel", typeof(RectTransform), typeof(Image));
            panelGo.transform.SetParent(transform, false);
            var panelRt = panelGo.GetComponent<RectTransform>();
            panelRt.anchorMin = Vector2.zero;
            panelRt.anchorMax = Vector2.one;
            panelRt.sizeDelta = Vector2.zero;
            panelGo.GetComponent<Image>().color = new Color(0.3f, 0.02f, 0.02f, 0.95f);
            _gameOverPanel = panelGo;

            var titleGo = new GameObject("GameOverTitle", typeof(RectTransform), typeof(Text));
            titleGo.transform.SetParent(panelGo.transform, false);
            var titleRt = titleGo.GetComponent<RectTransform>();
            titleRt.anchoredPosition = new Vector2(0f, 60f);
            titleRt.sizeDelta = new Vector2(500f, 80f);
            var titleTxt = titleGo.GetComponent<Text>();
            titleTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (titleTxt.font == null) titleTxt.font = Font.CreateDynamicFontFromOSFont("Arial", 42);
            titleTxt.fontSize = 42;
            titleTxt.fontStyle = FontStyle.Bold;
            titleTxt.color = Color.red;
            titleTxt.alignment = TextAnchor.MiddleCenter;
            titleTxt.text = "YOU ARE DEAD";

            var subGo = new GameObject("GameOverSubtitle", typeof(RectTransform), typeof(Text));
            subGo.transform.SetParent(panelGo.transform, false);
            var subRt = subGo.GetComponent<RectTransform>();
            subRt.anchoredPosition = new Vector2(0f, 0f);
            subRt.sizeDelta = new Vector2(500f, 40f);
            var subTxt = subGo.GetComponent<Text>();
            subTxt.font = titleTxt.font;
            subTxt.fontSize = 18;
            subTxt.fontStyle = FontStyle.Normal;
            subTxt.color = Color.white;
            subTxt.alignment = TextAnchor.MiddleCenter;
            subTxt.text = "El grupo de supervivientes ha caído en el S.S. Starlight.";

            var retryBtnGo = new GameObject("RetryButton", typeof(RectTransform), typeof(Image), typeof(Button));
            retryBtnGo.transform.SetParent(panelGo.transform, false);
            var retryRt = retryBtnGo.GetComponent<RectTransform>();
            retryRt.anchoredPosition = new Vector2(0f, -80f);
            retryRt.sizeDelta = new Vector2(220f, 45f);
            retryBtnGo.GetComponent<Image>().color = new Color(0.6f, 0.1f, 0.1f, 1f);
            retryBtnGo.GetComponent<Button>().onClick.AddListener(RestartGame);

            var btnTxtGo = new GameObject("ButtonText", typeof(RectTransform), typeof(Text));
            btnTxtGo.transform.SetParent(retryBtnGo.transform, false);
            var btnTxtRt = btnTxtGo.GetComponent<RectTransform>();
            btnTxtRt.sizeDelta = retryRt.sizeDelta;
            var btnTxt = btnTxtGo.GetComponent<Text>();
            btnTxt.font = titleTxt.font;
            btnTxt.fontSize = 18;
            btnTxt.fontStyle = FontStyle.Bold;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.text = "REINTENTAR";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using Game.Core;
using Game.Units;
using Game.Items;
using Game.Weapons;

namespace Game.UI {
    /// <summary>
    /// HUD de exploración visible en el modo top-down.
    /// Muestra la salud del personaje activo, estado alterado (veneno),
    /// arma equipada, contador de munición e indicadores de control.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class ExplorationHUD : MonoBehaviour {
        [Header("Contenedor")]
        [SerializeField] private GameObject _hudPanel;

        [Header("Textos y Barras")]
        [SerializeField] private Text _heroNameText;
        [SerializeField] private Slider _heroHpSlider;
        [SerializeField] private Text _heroHpText;
        [SerializeField] private Text _statusText;
        [SerializeField] private Text _weaponText;
        [SerializeField] private Text _ammoText;
        [SerializeField] private Text _controlsHintText;

        private void Awake() {
            EnsureUIHierarchy();
        }

        private void OnEnable() {
            GameManager.OnAfterStateChanged += HandleStateChanged;
            PartyManager.OnActiveHeroChanged += HandleActiveHeroChanged;
            PartyManager.OnHeroDamaged += HandleHeroDamaged;
            InventorySystem.OnInventoryUpdated += HandleInventoryUpdated;
        }

        private void OnDisable() {
            GameManager.OnAfterStateChanged -= HandleStateChanged;
            PartyManager.OnActiveHeroChanged -= HandleActiveHeroChanged;
            PartyManager.OnHeroDamaged -= HandleHeroDamaged;
            InventorySystem.OnInventoryUpdated -= HandleInventoryUpdated;
        }

        private void Start() {
            RefreshAll();
        }

        private void HandleStateChanged(GameState newState) {
            bool isExploration = (newState == GameState.Exploration);
            if (_hudPanel != null) {
                _hudPanel.SetActive(isExploration);
            }
            if (isExploration) {
                RefreshAll();
            }
        }

        private void HandleActiveHeroChanged(PartyMember member) => RefreshHeroInfo();
        private void HandleHeroDamaged(PartyMember member) => RefreshHeroInfo();
        private void HandleInventoryUpdated() => RefreshWeaponInfo();

        private void Update() {
            // Refresco suave por si el inventario o la vida cambian externamente
            if (_hudPanel != null && _hudPanel.activeSelf) {
                RefreshHeroInfo();
                RefreshWeaponInfo();
            }
        }

        public void RefreshAll() {
            RefreshHeroInfo();
            RefreshWeaponInfo();
        }

        private void RefreshHeroInfo() {
            var party = PartyManager.Instance;
            if (party == null || party.ActiveHero == null) return;

            var hero = party.ActiveHero;
            if (_heroNameText != null) _heroNameText.text = hero.Name.ToUpper();

            if (_heroHpSlider != null) {
                _heroHpSlider.maxValue = hero.MaxHp;
                _heroHpSlider.value = hero.CurrentHp;
            }

            if (_heroHpText != null) {
                _heroHpText.text = $"{hero.CurrentHp} / {hero.MaxHp} HP";
            }

            if (_statusText != null) {
                if (hero.IsPoisoned) {
                    _statusText.text = "ESTADO: [VENENO]";
                    _statusText.color = Color.green;
                } else if (hero.CurrentHp <= 25) {
                    _statusText.text = "ESTADO: [PELIGRO]";
                    _statusText.color = Color.red;
                } else if (hero.CurrentHp <= 50) {
                    _statusText.text = "ESTADO: [PRECAUCIÓN]";
                    _statusText.color = Color.yellow;
                } else {
                    _statusText.text = "ESTADO: [BIEN]";
                    _statusText.color = Color.white;
                }
            }
        }

        private void RefreshWeaponInfo() {
            var inv = InventorySystem.Instance;
            if (inv == null) return;

            var w = inv.EquippedWeapon;
            if (w != null) {
                if (_weaponText != null) _weaponText.text = $"EQUIPADA: {w.WeaponName.ToUpper()}";

                if (_ammoText != null) {
                    if (w.WeaponType == WeaponType.MeleeKnife) {
                        _ammoText.text = "MUNICIÓN: ∞";
                        _ammoText.color = Color.white;
                    } else {
                        int ammo = inv.GetAmmo(w.WeaponType);
                        _ammoText.text = $"MUNICIÓN: {ammo}";
                        _ammoText.color = ammo > 0 ? Color.white : Color.red;
                    }
                }
            }
        }

        public void EnsureUIHierarchy() {
            if (_hudPanel != null) return;

            var existing = transform.Find("ExplorationHUDPanel");
            if (existing != null) {
                _hudPanel = existing.gameObject;
                return;
            }

            // Panel Superior Izquierdo
            var panelGo = new GameObject("ExplorationHUDPanel", typeof(RectTransform));
            panelGo.transform.SetParent(transform, false);
            var panelRt = panelGo.GetComponent<RectTransform>();
            panelRt.anchorMin = new Vector2(0f, 1f);
            panelRt.anchorMax = new Vector2(0f, 1f);
            panelRt.pivot = new Vector2(0f, 1f);
            panelRt.anchoredPosition = new Vector2(25f, -25f);
            panelRt.sizeDelta = new Vector2(340f, 160f);
            _hudPanel = panelGo;

            // Fondo semitransparente oscuro
            var bgGo = new GameObject("Background", typeof(RectTransform), typeof(Image));
            bgGo.transform.SetParent(panelGo.transform, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.sizeDelta = Vector2.zero;
            bgGo.GetComponent<Image>().color = new Color(0.05f, 0.05f, 0.08f, 0.75f);

            // Nombre del Héroe
            var nameGo = CreateText("HeroNameText", panelGo.transform, new Vector2(15f, -15f), new Vector2(250f, 25f), 18, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft);
            _heroNameText = nameGo.GetComponent<Text>();

            // Slider de Vida
            var sliderGo = CreateSimpleSlider("HeroHpBar", panelGo.transform, new Vector2(15f, -45f), new Vector2(220f, 18f), new Color(0.2f, 0.8f, 0.3f));
            _heroHpSlider = sliderGo.GetComponent<Slider>();

            // Texto de Vida
            var hpTextGo = CreateText("HeroHpText", panelGo.transform, new Vector2(245f, -45f), new Vector2(80f, 18f), 13, FontStyle.Normal, Color.white, TextAnchor.MiddleLeft);
            _heroHpText = hpTextGo.GetComponent<Text>();

            // Estado (Bien / Veneno)
            var statusGo = CreateText("StatusText", panelGo.transform, new Vector2(15f, -70f), new Vector2(250f, 20f), 14, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft);
            _statusText = statusGo.GetComponent<Text>();

            // Arma y Munición
            var weaponGo = CreateText("WeaponText", panelGo.transform, new Vector2(15f, -95f), new Vector2(200f, 22f), 15, FontStyle.Bold, Color.yellow, TextAnchor.MiddleLeft);
            _weaponText = weaponGo.GetComponent<Text>();

            var ammoGo = CreateText("AmmoText", panelGo.transform, new Vector2(15f, -120f), new Vector2(200f, 22f), 15, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft);
            _ammoText = ammoGo.GetComponent<Text>();

            // Panel Inferior de Controles
            var hintGo = CreateText("ControlsHintText", transform, new Vector2(0f, 20f), new Vector2(800f, 30f), 14, FontStyle.Normal, new Color(0.9f, 0.9f, 0.9f, 0.85f), TextAnchor.MiddleCenter);
            var hintRt = hintGo.GetComponent<RectTransform>();
            hintRt.anchorMin = new Vector2(0.5f, 0f);
            hintRt.anchorMax = new Vector2(0.5f, 0f);
            hintRt.pivot = new Vector2(0.5f, 0f);
            _controlsHintText = hintGo.GetComponent<Text>();
            _controlsHintText.text = "[WASD / Stick] Mover  |  [Shift / B] Sprint  |  [E / A] Interactuar  |  [I / Select] Inventario";
        }

        private GameObject CreateText(string name, Transform parent, Vector2 pos, Vector2 size, int fontSize, FontStyle style, Color color, TextAnchor anchor) {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
            var txt = go.GetComponent<Text>();
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (txt.font == null) txt.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
            txt.fontSize = fontSize;
            txt.fontStyle = style;
            txt.color = color;
            txt.alignment = anchor;
            txt.raycastTarget = false;
            return go;
        }

        private GameObject CreateSimpleSlider(string name, Transform parent, Vector2 pos, Vector2 size, Color fillCol) {
            var sliderGo = new GameObject(name, typeof(RectTransform), typeof(Slider));
            sliderGo.transform.SetParent(parent, false);
            var rt = sliderGo.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;

            var bgGo = new GameObject("Background", typeof(RectTransform), typeof(Image));
            bgGo.transform.SetParent(sliderGo.transform, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.sizeDelta = Vector2.zero;
            bgGo.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            var fillArea = new GameObject("Fill Area", typeof(RectTransform));
            fillArea.transform.SetParent(sliderGo.transform, false);
            var faRt = fillArea.GetComponent<RectTransform>();
            faRt.anchorMin = Vector2.zero;
            faRt.anchorMax = Vector2.one;
            faRt.sizeDelta = Vector2.zero;

            var fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fill.transform.SetParent(fillArea.transform, false);
            var fillRt = fill.GetComponent<RectTransform>();
            fillRt.anchorMin = Vector2.zero;
            fillRt.anchorMax = Vector2.one;
            fillRt.sizeDelta = Vector2.zero;
            var fillImg = fill.GetComponent<Image>();
            fillImg.color = fillCol;

            var slider = sliderGo.GetComponent<Slider>();
            slider.fillRect = fillRt;
            slider.targetGraphic = fillImg;
            slider.direction = Slider.Direction.LeftToRight;
            slider.interactable = false;
            return sliderGo;
        }
    }
}

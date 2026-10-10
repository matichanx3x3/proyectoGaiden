using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;
using Game.Combat;
using Game.Units;
using Game.Weapons;
using Game.Items;

namespace Game.UI {
    /// <summary>
    /// Interfaz visual y controlador del minijuego de combate táctico en 1ª persona.
    /// Renderiza la barra de sincronización (0..120px), aguja oscilante, zonas de impacto/crítico,
    /// vida del enemigo, cadencia de ataque y mensajes de retroalimentación.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class CombatUI : MonoBehaviour {
        [Header("Contenedor Principal")]
        [SerializeField] private GameObject _combatPanel;

        [Header("Elementos del Retículo")]
        [SerializeField] private RectTransform _gaugeContainer;
        [SerializeField] private RectTransform _hitZoneRect;
        [SerializeField] private RectTransform _critZoneRect;
        [SerializeField] private RectTransform _needleRect;

        [Header("Información del Enemigo")]
        [SerializeField] private Text _enemyNameText;
        [SerializeField] private Slider _enemyHpSlider;
        [SerializeField] private Text _enemyHpText;
        [SerializeField] private Slider _enemyAttackTimerSlider;
        [SerializeField] private Image _enemyPortraitImage;

        [Header("Información del Jugador")]
        [SerializeField] private Text _weaponText;
        [SerializeField] private Text _ammoText;
        [SerializeField] private Text _heroHpText;
        [SerializeField] private Text _feedbackText;

        private Canvas _canvas;
        private Coroutine _feedbackCoroutine;

        private void Awake() {
            _canvas = GetComponent<Canvas>();
            EnsureUIHierarchy();
        }

        private void OnEnable() {
            GameManager.OnAfterStateChanged += HandleStateChanged;
            CombatManager.OnEnemyHit += HandleEnemyHit;
            CombatManager.OnShotFired += HandleShotFired;
            CombatManager.OnPlayerDamaged += HandlePlayerDamaged;
            CombatManager.OnCombatEnded += HandleCombatEnded;
        }

        private void OnDisable() {
            GameManager.OnAfterStateChanged -= HandleStateChanged;
            CombatManager.OnEnemyHit -= HandleEnemyHit;
            CombatManager.OnShotFired -= HandleShotFired;
            CombatManager.OnPlayerDamaged -= HandlePlayerDamaged;
            CombatManager.OnCombatEnded -= HandleCombatEnded;
        }

        private void Start() {
            bool isCombat = GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Combat;
            if (_combatPanel != null) {
                _combatPanel.SetActive(isCombat);
            }
            if (isCombat) {
                SetupCombatVisuals();
            }
        }

        private void HandleStateChanged(GameState newState) {
            bool isCombat = (newState == GameState.Combat);
            if (_combatPanel != null) {
                _combatPanel.SetActive(isCombat);
            }

            if (isCombat) {
                SetupCombatVisuals();
            }
        }

        private void SetupCombatVisuals() {
            var cm = CombatManager.Instance;
            if (cm == null || cm.CurrentEnemy == null) return;

            if (_enemyNameText != null) {
                _enemyNameText.text = cm.CurrentEnemy.EnemyName.ToUpper();
            }

            if (_enemyHpSlider != null) {
                _enemyHpSlider.maxValue = cm.CurrentEnemy.MaxHealth;
                _enemyHpSlider.value = cm.CurrentEnemyHp;
            }

            if (_enemyHpText != null) {
                _enemyHpText.text = $"{cm.CurrentEnemyHp} / {cm.CurrentEnemy.MaxHealth}";
            }

            if (_enemyPortraitImage != null) {
                if (cm.CurrentEnemy.BattleSprite != null) {
                    _enemyPortraitImage.sprite = cm.CurrentEnemy.BattleSprite;
                    _enemyPortraitImage.color = Color.white;
                } else {
                    _enemyPortraitImage.color = Color.clear;
                }
            }

            UpdateWeaponInfo();
            UpdateHeroHp();
            ShowFeedback("¡COMBATE INICIADO!", Color.yellow, 1.2f);
        }

        private void Update() {
            var cm = CombatManager.Instance;
            if (cm == null || _combatPanel == null || !_combatPanel.activeSelf) return;

            // 1. Actualizar posición de la aguja sobre el Gauge
            if (_gaugeContainer != null && _needleRect != null) {
                float totalWidth = _gaugeContainer.rect.width;
                float maxGauge = 120f;
                float scale = totalWidth / maxGauge;

                // 0..120 centrado en 60
                float needleX = (cm.NeedlePosition - 60f) * scale;
                _needleRect.anchoredPosition = new Vector2(needleX, 0f);

                // Ajustar ancho de zonas según el enemigo activo
                if (cm.CurrentEnemy != null) {
                    if (_hitZoneRect != null) {
                        float hitWidth = cm.CurrentEnemy.HitHalfWidth * 2f * scale;
                        _hitZoneRect.sizeDelta = new Vector2(hitWidth, _gaugeContainer.rect.height);
                    }
                    if (_critZoneRect != null) {
                        float critWidth = cm.CurrentEnemy.CritHalfWidth * 2f * scale;
                        _critZoneRect.sizeDelta = new Vector2(critWidth, _gaugeContainer.rect.height);
                    }
                }
            }

            // 2. Temporizador de turno del enemigo
            if (_enemyAttackTimerSlider != null && cm.CurrentEnemy != null) {
                float maxTime = cm.CurrentEnemy.AttackIntervalFrames / 60f;
                _enemyAttackTimerSlider.maxValue = maxTime;
                _enemyAttackTimerSlider.value = Mathf.Max(0f, cm.EnemyTurnTimer);
            }

            // 3. Refrescar textos de vida y arma
            if (_enemyHpSlider != null) _enemyHpSlider.value = cm.CurrentEnemyHp;
            if (_enemyHpText != null && cm.CurrentEnemy != null) {
                _enemyHpText.text = $"{cm.CurrentEnemyHp} / {cm.CurrentEnemy.MaxHealth}";
            }

            UpdateWeaponInfo();
            UpdateHeroHp();
        }

        private void UpdateWeaponInfo() {
            var inv = InventorySystem.Instance;
            if (inv == null || inv.EquippedWeapon == null) return;

            var w = inv.EquippedWeapon;
            if (_weaponText != null) _weaponText.text = $"ARMA: {w.WeaponName.ToUpper()}";

            if (_ammoText != null) {
                if (w.WeaponType == WeaponType.MeleeKnife) {
                    _ammoText.text = "MUNICIÓN: ∞";
                } else {
                    int ammo = inv.GetAmmo(w.WeaponType);
                    _ammoText.text = $"MUNICIÓN: {ammo}";
                    _ammoText.color = ammo > 0 ? Color.white : Color.red;
                }
            }
        }

        private void UpdateHeroHp() {
            var party = PartyManager.Instance;
            if (party != null && party.ActiveHero != null && _heroHpText != null) {
                var hero = party.ActiveHero;
                _heroHpText.text = $"{hero.Name.ToUpper()}: {hero.CurrentHp}/{hero.MaxHp} HP" + (hero.IsPoisoned ? " [VENENO]" : "");
                _heroHpText.color = hero.IsPoisoned ? Color.green : (hero.CurrentHp < 30 ? Color.red : Color.white);
            }
        }

        private void HandleEnemyHit(HitOutcome outcome, int damage) {
            switch (outcome) {
                case HitOutcome.Critical:
                    ShowFeedback($"¡IMPACTO CRÍTICO! -{damage} HP", Color.red, 1.0f);
                    break;
                case HitOutcome.Normal:
                    ShowFeedback($"IMPACTO NORMAL -{damage} HP", Color.yellow, 0.8f);
                    break;
                case HitOutcome.Miss:
                    ShowFeedback("¡DISPARO FALLIDO! (MISS)", Color.gray, 0.8f);
                    break;
            }
        }

        private void HandleShotFired(ScriptableWeapon weapon, bool hasAmmo) {
            if (!hasAmmo) {
                ShowFeedback("¡SIN MUNICIÓN! *CLICK*", Color.red, 0.8f);
            }
        }

        private void HandlePlayerDamaged(int damage) {
            ShowFeedback($"¡EL ENEMIGO ATACA! -{damage} HP", new Color(1f, 0.3f, 0.3f), 1.0f);
        }

        private void HandleCombatEnded() {
            ShowFeedback("¡ENEMIGO DERROTADO! VICTORIA", Color.cyan, 1.5f);
        }

        private void ShowFeedback(string message, Color color, float duration) {
            if (_feedbackText == null) return;
            if (_feedbackCoroutine != null) StopCoroutine(_feedbackCoroutine);
            _feedbackCoroutine = StartCoroutine(FeedbackRoutine(message, color, duration));
        }

        private IEnumerator FeedbackRoutine(string message, Color color, float duration) {
            _feedbackText.text = message;
            _feedbackText.color = color;
            _feedbackText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _feedbackText.text = "";
        }

        // =========================================================
        // AUTOCREACIÓN DE JERARQUÍA UI SI NO ESTÁ CONFIGURADA EN INSPECTOR
        // =========================================================
        public void EnsureUIHierarchy() {
            if (_combatPanel != null) return;

            var existingPanel = transform.Find("CombatPanel");
            if (existingPanel != null) {
                _combatPanel = existingPanel.gameObject;
                return;
            }

            // Crear Panel Oscuro de Fondo
            var panelGo = new GameObject("CombatPanel", typeof(RectTransform), typeof(Image));
            panelGo.transform.SetParent(transform, false);
            var panelRect = panelGo.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;
            var panelImg = panelGo.GetComponent<Image>();
            panelImg.color = new Color(0.02f, 0.02f, 0.04f, 0.25f); // Fondo translúcido para apreciar la arena 3D
            _combatPanel = panelGo;

            // Marco del Enemigo (Centro Superior)
            var enemyBox = CreateTextElement("EnemyNameText", panelGo.transform, new Vector2(0f, 220f), new Vector2(400f, 40f), 24, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
            _enemyNameText = enemyBox.GetComponent<Text>();

            // Barra de Vida Enemigo
            var hpSliderGo = CreateSimpleSlider("EnemyHpBar", panelGo.transform, new Vector2(0f, 185f), new Vector2(300f, 20f), new Color(0.8f, 0.1f, 0.1f));
            _enemyHpSlider = hpSliderGo.GetComponent<Slider>();

            var hpTextGo = CreateTextElement("EnemyHpText", panelGo.transform, new Vector2(0f, 160f), new Vector2(300f, 25f), 16, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
            _enemyHpText = hpTextGo.GetComponent<Text>();

            // Imagen / Silueta del Enemigo (Oculta por defecto en arena 3D salvo que tenga sprite 2D explícito)
            var enemyPortraitGo = new GameObject("EnemyPortrait", typeof(RectTransform), typeof(Image));
            enemyPortraitGo.transform.SetParent(panelGo.transform, false);
            var portRect = enemyPortraitGo.GetComponent<RectTransform>();
            portRect.anchoredPosition = new Vector2(0f, 50f);
            portRect.sizeDelta = new Vector2(160f, 160f);
            var portImg = enemyPortraitGo.GetComponent<Image>();
            portImg.color = Color.clear;
            _enemyPortraitImage = portImg;

            // Barra de Tiempo de Ataque Enemigo
            var timerSliderGo = CreateSimpleSlider("EnemyAttackTimerBar", panelGo.transform, new Vector2(0f, -50f), new Vector2(250f, 10f), new Color(1f, 0.6f, 0.1f));
            _enemyAttackTimerSlider = timerSliderGo.GetComponent<Slider>();

            // Texto de Retroalimentación de Disparo
            var feedbackGo = CreateTextElement("FeedbackText", panelGo.transform, new Vector2(0f, -85f), new Vector2(500f, 40f), 22, FontStyle.Bold, Color.yellow, TextAnchor.MiddleCenter);
            _feedbackText = feedbackGo.GetComponent<Text>();
            _feedbackText.text = "";

            // =====================================================
            // CONTENEDOR DEL RETÍCULO (GAUGE 0..120 PX)
            // =====================================================
            var gaugeBox = new GameObject("GaugeContainer", typeof(RectTransform), typeof(Image));
            gaugeBox.transform.SetParent(panelGo.transform, false);
            _gaugeContainer = gaugeBox.GetComponent<RectTransform>();
            _gaugeContainer.anchoredPosition = new Vector2(0f, -160f);
            _gaugeContainer.sizeDelta = new Vector2(400f, 40f); // Ancho en UI
            var gaugeBg = gaugeBox.GetComponent<Image>();
            gaugeBg.color = new Color(0.15f, 0.15f, 0.15f, 1f);

            // Zona de Impacto Normal (Amarillo transparente)
            var hitZoneGo = new GameObject("HitZone", typeof(RectTransform), typeof(Image));
            hitZoneGo.transform.SetParent(_gaugeContainer, false);
            _hitZoneRect = hitZoneGo.GetComponent<RectTransform>();
            _hitZoneRect.anchoredPosition = Vector2.zero;
            _hitZoneRect.sizeDelta = new Vector2(120f, 40f);
            var hitImg = hitZoneGo.GetComponent<Image>();
            hitImg.color = new Color(1f, 0.85f, 0.1f, 0.45f);

            // Diana Crítica (Rojo transparente)
            var critZoneGo = new GameObject("CritZone", typeof(RectTransform), typeof(Image));
            critZoneGo.transform.SetParent(_gaugeContainer, false);
            _critZoneRect = critZoneGo.GetComponent<RectTransform>();
            _critZoneRect.anchoredPosition = Vector2.zero;
            _critZoneRect.sizeDelta = new Vector2(25f, 40f);
            var critImg = critZoneGo.GetComponent<Image>();
            critImg.color = new Color(1f, 0.15f, 0.15f, 0.85f);

            // Marca Central
            var centerLineGo = new GameObject("CenterMarker", typeof(RectTransform), typeof(Image));
            centerLineGo.transform.SetParent(_gaugeContainer, false);
            var centerRect = centerLineGo.GetComponent<RectTransform>();
            centerRect.anchoredPosition = Vector2.zero;
            centerRect.sizeDelta = new Vector2(2f, 40f);
            centerLineGo.GetComponent<Image>().color = Color.white;

            // Aguja Blanca Oscilante
            var needleGo = new GameObject("Needle", typeof(RectTransform), typeof(Image));
            needleGo.transform.SetParent(_gaugeContainer, false);
            _needleRect = needleGo.GetComponent<RectTransform>();
            _needleRect.anchoredPosition = Vector2.zero;
            _needleRect.sizeDelta = new Vector2(6f, 52f);
            var needleImg = needleGo.GetComponent<Image>();
            needleImg.color = Color.white;

            // Información de Arma, Munición y Héroe en la parte inferior
            var weaponGo = CreateTextElement("WeaponText", panelGo.transform, new Vector2(-150f, -220f), new Vector2(280f, 30f), 18, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft);
            _weaponText = weaponGo.GetComponent<Text>();

            var ammoGo = CreateTextElement("AmmoText", panelGo.transform, new Vector2(-150f, -250f), new Vector2(280f, 30f), 18, FontStyle.Bold, Color.white, TextAnchor.MiddleLeft);
            _ammoText = ammoGo.GetComponent<Text>();

            var heroHpGo = CreateTextElement("HeroHpText", panelGo.transform, new Vector2(150f, -220f), new Vector2(280f, 30f), 18, FontStyle.Bold, Color.white, TextAnchor.MiddleRight);
            _heroHpText = heroHpGo.GetComponent<Text>();

            // Instrucciones / Controles
            var controlsGo = CreateTextElement("ControlsText", panelGo.transform, new Vector2(0f, -280f), new Vector2(600f, 30f), 14, FontStyle.Italic, new Color(0.8f, 0.8f, 0.8f), TextAnchor.MiddleCenter);
            controlsGo.GetComponent<Text>().text = "[Espacio / Clic / Gatillo] Disparar   |   [Q / RB] Cambiar Arma   |   [Tab / LB] Miembro Party";
        }

        private GameObject CreateTextElement(string name, Transform parent, Vector2 pos, Vector2 size, int fontSize, FontStyle style, Color color, TextAnchor anchor) {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
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
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;

            // Background
            var bgGo = new GameObject("Background", typeof(RectTransform), typeof(Image));
            bgGo.transform.SetParent(sliderGo.transform, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.sizeDelta = Vector2.zero;
            bgGo.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Fill Area
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

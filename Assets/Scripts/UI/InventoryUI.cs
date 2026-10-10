using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Game.Core;
using Game.Items;
using Game.Units;

namespace Game.UI {
    /// <summary>
    /// Menú de gestión de inventario de 8 casillas.
    /// Permite inspeccionar objetos, usar botiquines/hierbas, soltar objetos al escenario (Drop),
    /// y volver a la fase de exploración.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class InventoryUI : MonoBehaviour {
        [Header("Contenedor Principal")]
        [SerializeField] private GameObject _inventoryPanel;

        [Header("Slots (8 Casillas)")]
        [SerializeField] private List<Text> _slotTexts = new List<Text>();

        [Header("Detalle del Objeto")]
        [SerializeField] private Text _itemDescriptionText;
        [SerializeField] private Text _partyStatusText;

        private float _openedTime;
        private int _selectedSlotIndex = 0;

        private void Awake() {
            EnsureUIHierarchy();
        }

        private void OnEnable() {
            GameManager.OnAfterStateChanged += HandleStateChanged;
            InventorySystem.OnInventoryUpdated += RefreshSlots;
        }

        private void OnDisable() {
            GameManager.OnAfterStateChanged -= HandleStateChanged;
            InventorySystem.OnInventoryUpdated -= RefreshSlots;
        }

        private void Start() {
            if (_inventoryPanel != null) {
                _inventoryPanel.SetActive(false);
            }
        }

        private void HandleStateChanged(GameState newState) {
            bool isInv = (newState == GameState.Inventory);
            if (_inventoryPanel != null) {
                _inventoryPanel.SetActive(isInv);
            }

            if (isInv) {
                _openedTime = Time.unscaledTime;
                RefreshSlots();
            }
        }

        private void Update() {
            if (_inventoryPanel == null || !_inventoryPanel.activeSelf) return;

            // Debounce de 0.25s para evitar cierre inmediato
            if (Time.unscaledTime - _openedTime < 0.25f) return;

            var keyboard = Keyboard.current;
            if (keyboard != null) {
                bool isShift = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;

                if (keyboard.digit1Key.wasPressedThisFrame) HandleSlotAction(0, isShift);
                else if (keyboard.digit2Key.wasPressedThisFrame) HandleSlotAction(1, isShift);
                else if (keyboard.digit3Key.wasPressedThisFrame) HandleSlotAction(2, isShift);
                else if (keyboard.digit4Key.wasPressedThisFrame) HandleSlotAction(3, isShift);
                else if (keyboard.digit5Key.wasPressedThisFrame) HandleSlotAction(4, isShift);
                else if (keyboard.digit6Key.wasPressedThisFrame) HandleSlotAction(5, isShift);
                else if (keyboard.digit7Key.wasPressedThisFrame) HandleSlotAction(6, isShift);
                else if (keyboard.digit8Key.wasPressedThisFrame) HandleSlotAction(7, isShift);

                // Tecla D para soltar el slot seleccionado o primer slot disponible
                if (keyboard.dKey.wasPressedThisFrame || keyboard.xKey.wasPressedThisFrame) {
                    DropSlot(_selectedSlotIndex);
                }

                // Cerrar menú con I, Tab o Escape
                if (keyboard.iKey.wasPressedThisFrame || keyboard.tabKey.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame) {
                    CloseInventory();
                }
            }

            var gamepad = Gamepad.current;
            if (gamepad != null) {
                if (gamepad.buttonEast.wasPressedThisFrame || gamepad.startButton.wasPressedThisFrame || gamepad.selectButton.wasPressedThisFrame) {
                    CloseInventory();
                }
                if (gamepad.buttonNorth.wasPressedThisFrame) {
                    DropSlot(_selectedSlotIndex);
                }
            }
        }

        private void HandleSlotAction(int index, bool isDrop) {
            _selectedSlotIndex = index;
            if (isDrop) {
                DropSlot(index);
            } else {
                UseSlot(index);
            }
        }

        public void CloseInventory() {
            if (GameManager.Instance != null) {
                GameManager.Instance.ChangeState(GameState.Exploration);
            }
        }

        public void UseSlot(int index) {
            _selectedSlotIndex = index;
            var inv = InventorySystem.Instance;
            if (inv == null || index >= inv.Slots.Count) return;

            ItemId item = inv.Slots[index];
            inv.UseItem(index);

            if (_itemDescriptionText != null) {
                _itemDescriptionText.text = $"Usado: {item}";
            }
            RefreshSlots();
        }

        public void DropSlot(int index) {
            var inv = InventorySystem.Instance;
            if (inv == null || index < 0 || index >= inv.Slots.Count) return;

            Vector3 dropPos = Vector3.zero;
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                dropPos = player.transform.position + player.transform.forward * 1.0f;
            }

            ItemId item = inv.Slots[index];
            bool dropped = inv.DropItem(index, dropPos);

            if (dropped && _itemDescriptionText != null) {
                _itemDescriptionText.text = $"Soltado al suelo: {item}";
            }
            RefreshSlots();
        }

        public void RefreshSlots() {
            var inv = InventorySystem.Instance;
            if (inv == null) return;

            var slots = inv.Slots;
            for (int i = 0; i < _slotTexts.Count; i++) {
                if (i < slots.Count) {
                    _slotTexts[i].text = $"[{i + 1}] {slots[i]}";
                    _slotTexts[i].color = (i == _selectedSlotIndex) ? Color.yellow : Color.white;
                } else {
                    _slotTexts[i].text = $"[{i + 1}] --- VACÍO ---";
                    _slotTexts[i].color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
                }
            }

            var party = PartyManager.Instance;
            if (party != null && party.ActiveHero != null && _partyStatusText != null) {
                var hero = party.ActiveHero;
                _partyStatusText.text = $"SALUD: {hero.Name} ({hero.CurrentHp}/{hero.MaxHp} HP)" + (hero.IsPoisoned ? " [¡ENFERMO DE VENENO!]" : "");
                _partyStatusText.color = hero.IsPoisoned ? Color.green : (hero.CurrentHp <= 30 ? Color.red : Color.white);
            }
        }

        public void EnsureUIHierarchy() {
            if (_inventoryPanel != null) return;

            var existing = transform.Find("InventoryPanel");
            if (existing != null) {
                _inventoryPanel = existing.gameObject;
                return;
            }

            var panelGo = new GameObject("InventoryPanel", typeof(RectTransform), typeof(Image));
            panelGo.transform.SetParent(transform, false);
            var panelRt = panelGo.GetComponent<RectTransform>();
            panelRt.anchorMin = new Vector2(0.5f, 0.5f);
            panelRt.anchorMax = new Vector2(0.5f, 0.5f);
            panelRt.pivot = new Vector2(0.5f, 0.5f);
            panelRt.anchoredPosition = Vector2.zero;
            panelRt.sizeDelta = new Vector2(520f, 440f);
            var panelImg = panelGo.GetComponent<Image>();
            panelImg.color = new Color(0.08f, 0.08f, 0.12f, 0.95f);
            _inventoryPanel = panelGo;

            var titleGo = CreateText("TitleText", panelGo.transform, new Vector2(0f, 185f), new Vector2(400f, 35f), 22, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
            titleGo.GetComponent<Text>().text = "MALETÍN DE SUMINISTROS (8 CASILLAS)";

            var statusGo = CreateText("PartyStatusText", panelGo.transform, new Vector2(0f, 150f), new Vector2(450f, 25f), 16, FontStyle.Normal, Color.white, TextAnchor.MiddleCenter);
            _partyStatusText = statusGo.GetComponent<Text>();

            _slotTexts.Clear();
            float startX = -125f;
            float startY = 95f;
            float stepX = 250f;
            float stepY = -48f;

            for (int i = 0; i < 8; i++) {
                int col = i % 2;
                int row = i / 2;
                Vector2 pos = new Vector2(startX + (col * stepX), startY + (row * stepY));

                var slotBtnGo = new GameObject($"SlotButton_{i}", typeof(RectTransform), typeof(Image), typeof(Button));
                slotBtnGo.transform.SetParent(panelGo.transform, false);
                var slotRt = slotBtnGo.GetComponent<RectTransform>();
                slotRt.anchoredPosition = pos;
                slotRt.sizeDelta = new Vector2(220f, 40f);
                var slotImg = slotBtnGo.GetComponent<Image>();
                slotImg.color = new Color(0.18f, 0.18f, 0.22f, 0.9f);

                int capturedIndex = i;
                slotBtnGo.GetComponent<Button>().onClick.AddListener(() => {
                    _selectedSlotIndex = capturedIndex;
                    UseSlot(capturedIndex);
                });

                var textGo = CreateText($"SlotText_{i}", slotBtnGo.transform, Vector2.zero, new Vector2(200f, 30f), 15, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
                var txt = textGo.GetComponent<Text>();
                _slotTexts.Add(txt);
            }

            var descGo = CreateText("ItemDescriptionText", panelGo.transform, new Vector2(0f, -115f), new Vector2(480f, 35f), 14, FontStyle.Italic, Color.yellow, TextAnchor.MiddleCenter);
            _itemDescriptionText = descGo.GetComponent<Text>();
            _itemDescriptionText.text = "Clic/[1-8]: Usar | Shift+Clic/[D]: Tirar al suelo";

            // Botón Tirar al Suelo
            var dropBtnGo = new GameObject("DropButton", typeof(RectTransform), typeof(Image), typeof(Button));
            dropBtnGo.transform.SetParent(panelGo.transform, false);
            var dropRt = dropBtnGo.GetComponent<RectTransform>();
            dropRt.anchoredPosition = new Vector2(-110f, -165f);
            dropRt.sizeDelta = new Vector2(170f, 35f);
            dropBtnGo.GetComponent<Image>().color = new Color(0.6f, 0.35f, 0.1f, 0.9f);
            dropBtnGo.GetComponent<Button>().onClick.AddListener(() => DropSlot(_selectedSlotIndex));

            var dropTxt = CreateText("DropText", dropBtnGo.transform, Vector2.zero, new Vector2(150f, 30f), 14, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
            dropTxt.GetComponent<Text>().text = "TIRAR [D / X]";

            // Botón Cerrar
            var closeBtnGo = new GameObject("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
            closeBtnGo.transform.SetParent(panelGo.transform, false);
            var closeRt = closeBtnGo.GetComponent<RectTransform>();
            closeRt.anchoredPosition = new Vector2(110f, -165f);
            closeRt.sizeDelta = new Vector2(170f, 35f);
            closeBtnGo.GetComponent<Image>().color = new Color(0.4f, 0.1f, 0.1f, 0.9f);
            closeBtnGo.GetComponent<Button>().onClick.AddListener(CloseInventory);

            var closeTxt = CreateText("CloseText", closeBtnGo.transform, Vector2.zero, new Vector2(150f, 30f), 14, FontStyle.Bold, Color.white, TextAnchor.MiddleCenter);
            closeTxt.GetComponent<Text>().text = "CERRAR [I / ESC]";
        }

        private GameObject CreateText(string name, Transform parent, Vector2 pos, Vector2 size, int fontSize, FontStyle style, Color color, TextAnchor anchor) {
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
    }
}

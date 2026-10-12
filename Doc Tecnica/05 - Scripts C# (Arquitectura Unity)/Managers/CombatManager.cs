using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Game.Core;
using Game.Weapons;
using Game.Units;
using Game.Items;
using Game.Managers;

namespace Game.Combat {
    public enum HitOutcome { Miss = 0, Normal = 1, Critical = 2 }

    /// <summary>
    /// Motor de combate basado en retículo oscilante y turnos tácticos.
    /// Utiliza el nuevo sistema de Input (UnityEngine.InputSystem).
    /// Carga la escena aditiva 03_CombatArena como instancia de combate separada (estilo JRPG/survival horror).
    /// </summary>
    public class CombatManager : Singleton<CombatManager> {
        public static event Action<ScriptableWeapon, bool> OnShotFired;
        public static event Action<HitOutcome, int> OnEnemyHit;
        public static event Action<int> OnPlayerDamaged;
        public static event Action OnCombatEnded;

        [Header("Calibración del Retículo")]
        [SerializeField] private float _gaugeMaxWidth = 120f;
        [SerializeField] private float _reticleSpeed = 180f;

        public ScriptableEnemy CurrentEnemy { get; private set; }
        public int CurrentEnemyHp { get; private set; }
        public float NeedlePosition { get; private set; }

        [Header("Instancia Aditiva de Combate")]
        [SerializeField] private string _combatSceneName = "03_CombatArena";

        public float TargetCenter { get; private set; } = 60f;
        public float EnemyTurnTimer { get; private set; }
        public bool IsCombatActive => _isActive;

        private int _needleDir = 1;
        private bool _isActive;
        private bool _isTransitioning;
        private EnemyOverworldUnit _overworldEnemyRef;
        private Camera _explorationCamera;
        private AudioListener _explorationAudioListener;

        public void StartCombat(EnemyType enemyType, EnemyOverworldUnit overworldRef) {
            if (_isActive || _isTransitioning) return;
            StartCoroutine(LoadCombatSceneRoutine(enemyType, overworldRef));
        }

        private IEnumerator LoadCombatSceneRoutine(EnemyType enemyType, EnemyOverworldUnit overworldRef) {
            _isTransitioning = true;

            if (ResourceSystem.Instance != null) {
                CurrentEnemy = ResourceSystem.Instance.GetEnemy(enemyType);
            }

            int maxHp = CurrentEnemy != null ? CurrentEnemy.MaxHealth : 50;
            int attackFrames = CurrentEnemy != null ? CurrentEnemy.AttackIntervalFrames : 180;

            CurrentEnemyHp = maxHp;
            _overworldEnemyRef = overworldRef;

            NeedlePosition = 0f;
            _needleDir = 1;
            EnemyTurnTimer = attackFrames / 60f;

            // Desactivar cámara y audio de exploración para dar paso a la arena 3D de combate
            _explorationCamera = Camera.main;
            if (_explorationCamera != null) {
                _explorationAudioListener = _explorationCamera.GetComponent<AudioListener>();
                _explorationCamera.enabled = false;
                if (_explorationAudioListener != null) {
                    _explorationAudioListener.enabled = false;
                }
            }

            // Cargar escena aditiva de combate si no está cargada
            var existingScene = SceneManager.GetSceneByName(_combatSceneName);
            if (!existingScene.isLoaded) {
                AsyncOperation asyncLoad = null;
                try {
                    asyncLoad = SceneManager.LoadSceneAsync(_combatSceneName, LoadSceneMode.Additive);
                } catch (Exception ex) {
                    Debug.LogWarning($"[CombatManager] Error iniciando carga de {_combatSceneName}: {ex.Message}");
                }

                if (asyncLoad != null) {
                    while (!asyncLoad.isDone) {
                        yield return null;
                    }
                }
            }

            var loadedCombatScene = SceneManager.GetSceneByName(_combatSceneName);
            if (loadedCombatScene.IsValid() && loadedCombatScene.isLoaded) {
                SceneManager.SetActiveScene(loadedCombatScene);
            }

            // Configurar director visual de la arena
            var director = FindFirstObjectByType<CombatArenaDirector>();
            if (director != null) {
                director.SetupArena();
            }

            _isActive = true;
            _isTransitioning = false;

            if (GameManager.Instance != null) {
                GameManager.Instance.ChangeState(GameState.Combat);
            }
        }

        private void Update() {
            if (!_isActive || _isTransitioning) return;

            UpdateReticle();
            UpdateEnemyTimer();
            HandleInput();
        }

        private void UpdateReticle() {
            NeedlePosition += _needleDir * _reticleSpeed * Time.deltaTime;
            if (NeedlePosition >= _gaugeMaxWidth) {
                NeedlePosition = _gaugeMaxWidth;
                _needleDir = -1;
            } else if (NeedlePosition <= 0f) {
                NeedlePosition = 0f;
                _needleDir = 1;
            }
        }

        private void UpdateEnemyTimer() {
            EnemyTurnTimer -= Time.deltaTime;
            if (EnemyTurnTimer <= 0f) {
                int intervalFrames = CurrentEnemy != null ? CurrentEnemy.AttackIntervalFrames : 180;
                int atkPower = CurrentEnemy != null ? CurrentEnemy.AttackPower : 15;

                EnemyTurnTimer = intervalFrames / 60f;
                if (PartyManager.Instance != null) {
                    PartyManager.Instance.DamageActiveHero(atkPower);
                }
                OnPlayerDamaged?.Invoke(atkPower);
            }
        }

        private void HandleInput() {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;
            var mouse = Mouse.current;

            // Disparar
            bool shootPressed = false;
            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame) shootPressed = true;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame) shootPressed = true;
            if (gamepad != null && (gamepad.buttonSouth.wasPressedThisFrame || gamepad.rightTrigger.wasPressedThisFrame)) shootPressed = true;

            if (shootPressed) {
                TriggerShot();
            }

            // Alternar Arma
            bool cycleWeaponPressed = false;
            if (keyboard != null && keyboard.qKey.wasPressedThisFrame) cycleWeaponPressed = true;
            if (mouse != null && mouse.rightButton.wasPressedThisFrame) cycleWeaponPressed = true;
            if (gamepad != null && gamepad.rightShoulder.wasPressedThisFrame) cycleWeaponPressed = true;

            if (cycleWeaponPressed) {
                if (InventorySystem.Instance != null) {
                    InventorySystem.Instance.CycleNextWeapon();
                }
            }

            // Alternar Miembro de la Party
            bool cycleHeroPressed = false;
            if (keyboard != null && keyboard.tabKey.wasPressedThisFrame) cycleHeroPressed = true;
            if (gamepad != null && gamepad.leftShoulder.wasPressedThisFrame) cycleHeroPressed = true;

            if (cycleHeroPressed) {
                if (PartyManager.Instance != null) {
                    PartyManager.Instance.CycleNextHero();
                }
            }
        }

        public void TriggerShot() {
            var inv = InventorySystem.Instance;
            var weapon = inv != null ? inv.EquippedWeapon : null;

            // Verificar munición
            if (weapon != null && weapon.WeaponType != WeaponType.MeleeKnife && inv != null && inv.GetAmmo(weapon.WeaponType) <= 0) {
                OnShotFired?.Invoke(weapon, false);
                return;
            }

            if (weapon != null && weapon.WeaponType != WeaponType.MeleeKnife && inv != null) {
                inv.ConsumeAmmo(weapon.WeaponType, 1);
            }
            OnShotFired?.Invoke(weapon, true);

            // Medir distancia a la diana
            float dist = Mathf.Abs(NeedlePosition - TargetCenter);
            HitOutcome outcome = HitOutcome.Miss;

            int critWidth = CurrentEnemy != null ? CurrentEnemy.CritHalfWidth : 4;
            int hitWidth = CurrentEnemy != null ? CurrentEnemy.HitHalfWidth : 18;

            if (dist <= critWidth) outcome = HitOutcome.Critical;
            else if (dist <= hitWidth) outcome = HitOutcome.Normal;

            int baseDmg = weapon != null ? weapon.BaseDamage : 10;
            int critMult = weapon != null ? weapon.CritMultiplier : 2;

            int damage = 0;
            if (outcome == HitOutcome.Critical) damage = baseDmg * critMult;
            else if (outcome == HitOutcome.Normal) damage = baseDmg;

            if (damage > 0) {
                CurrentEnemyHp = Mathf.Max(0, CurrentEnemyHp - damage);
            }

            OnEnemyHit?.Invoke(outcome, damage);

            if (CurrentEnemyHp <= 0) {
                EndCombat(victory: true);
            }
        }

        public void EndCombat(bool victory) {
            if (!_isActive || _isTransitioning) return;
            _isActive = false;
            StartCoroutine(UnloadCombatSceneRoutine(victory));
        }

        private IEnumerator UnloadCombatSceneRoutine(bool victory) {
            _isTransitioning = true;
            OnCombatEnded?.Invoke();

            // Esperar brevemente para retroalimentación visual de victoria / desenlace
            yield return new WaitForSeconds(1.2f);

            // Eliminar enemigo del overworld si fue derrotado
            if (victory && _overworldEnemyRef != null) {
                if (UnitManager.Instance != null) {
                    UnitManager.Instance.RemoveEnemy(_overworldEnemyRef);
                } else {
                    Destroy(_overworldEnemyRef.gameObject);
                }
                _overworldEnemyRef = null;
            }

            // Descargar escena aditiva de combate
            var combatScene = SceneManager.GetSceneByName(_combatSceneName);
            if (combatScene.IsValid() && combatScene.isLoaded) {
                AsyncOperation asyncUnload = null;
                try {
                    asyncUnload = SceneManager.UnloadSceneAsync(_combatSceneName);
                } catch (Exception ex) {
                    Debug.LogWarning($"[CombatManager] Error descargando {_combatSceneName}: {ex.Message}");
                }

                if (asyncUnload != null) {
                    while (!asyncUnload.isDone) {
                        yield return null;
                    }
                }
            }

            // Restaurar cámara de exploración
            if (_explorationCamera != null) {
                _explorationCamera.enabled = true;
                if (_explorationAudioListener != null) {
                    _explorationAudioListener.enabled = true;
                }
            }

            // Restaurar escena activa a exploración
            var expScene = SceneManager.GetSceneByName("02_Ship_DeckA");
            if (expScene.IsValid() && expScene.isLoaded) {
                SceneManager.SetActiveScene(expScene);
            }

            _isTransitioning = false;

            if (GameManager.Instance != null) {
                GameManager.Instance.ChangeState(GameState.Exploration);
            }
        }
    }
}

using System;
using UnityEngine;
using Gaiden.Core;
using Gaiden.Weapons;
using Gaiden.Units;
using Gaiden.Items;

namespace Gaiden.Combat {
    public enum HitOutcome { Miss = 0, Normal = 1, Critical = 2 }

    /// <summary>
    /// Motor de combate basado en retículo oscilante y turnos de Resident Evil Gaiden.
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
        public float TargetCenter { get; private set; } = 60f;
        public float EnemyTurnTimer { get; private set; }

        private int _needleDir = 1;
        private bool _isActive;
        private EnemyOverworldUnit _overworldEnemyRef;

        public void StartCombat(EnemyType enemyType, EnemyOverworldUnit overworldRef) {
            CurrentEnemy = ResourceSystem.Instance.GetEnemy(enemyType);
            CurrentEnemyHp = CurrentEnemy.MaxHealth;
            _overworldEnemyRef = overworldRef;

            NeedlePosition = 0f;
            _needleDir = 1;
            EnemyTurnTimer = CurrentEnemy.AttackIntervalFrames / 60f;
            _isActive = true;

            GaidenGameManager.Instance.ChangeState(GaidenGameState.Combat);
        }

        private void Update() {
            if (!_isActive) return;

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
                EnemyTurnTimer = CurrentEnemy.AttackIntervalFrames / 60f;
                PartyManager.Instance.DamageActiveHero(CurrentEnemy.AttackPower);
                OnPlayerDamaged?.Invoke(CurrentEnemy.AttackPower);
            }
        }

        private void HandleInput() {
            // Disparar
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) {
                TriggerShot();
            }

            // Alternar Arma
            if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Q)) {
                InventorySystem.Instance.CycleNextWeapon();
            }

            // Alternar Miembro de la Party
            if (Input.GetKeyDown(KeyCode.Tab)) {
                PartyManager.Instance.CycleNextHero();
            }
        }

        public void TriggerShot() {
            var weapon = InventorySystem.Instance.EquippedWeapon;
            if (weapon == null) return;

            // Verificar munición
            if (weapon.WeaponType != WeaponType.Knife && InventorySystem.Instance.GetAmmo(weapon.WeaponType) <= 0) {
                OnShotFired?.Invoke(weapon, false);
                return;
            }

            if (weapon.WeaponType != WeaponType.Knife) {
                InventorySystem.Instance.ConsumeAmmo(weapon.WeaponType, 1);
            }
            OnShotFired?.Invoke(weapon, true);

            // Medir distancia a la diana
            float dist = Mathf.Abs(NeedlePosition - TargetCenter);
            HitOutcome outcome = HitOutcome.Miss;

            if (dist <= CurrentEnemy.CritHalfWidth) outcome = HitOutcome.Critical;
            else if (dist <= CurrentEnemy.HitHalfWidth) outcome = HitOutcome.Normal;

            int damage = 0;
            if (outcome == HitOutcome.Critical) damage = weapon.BaseDamage * weapon.CritMultiplier;
            else if (outcome == HitOutcome.Normal) damage = weapon.BaseDamage;

            if (damage > 0) {
                CurrentEnemyHp = Mathf.Max(0, CurrentEnemyHp - damage);
            }

            OnEnemyHit?.Invoke(outcome, damage);

            if (CurrentEnemyHp <= 0) {
                EndCombat(victory: true);
            }
        }

        private void EndCombat(bool victory) {
            _isActive = false;
            if (victory && _overworldEnemyRef != null) {
                UnitManager.Instance.RemoveEnemy(_overworldEnemyRef);
            }
            OnCombatEnded?.Invoke();
            GaidenGameManager.Instance.ChangeState(GaidenGameState.Exploration);
        }
    }
}

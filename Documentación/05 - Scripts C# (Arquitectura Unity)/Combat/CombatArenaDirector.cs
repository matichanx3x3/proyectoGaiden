using System.Collections;
using UnityEngine;
using Game.Units;
using Game.Combat;

namespace Game.Combat {
    /// <summary>
    /// Director de la escena aditiva de combate (03_CombatArena).
    /// Instancia la vista en 1ª persona/dramática del enemigo, la iluminación de tensión,
    /// las sacudidas de cámara por impacto y el decorado del encuentro.
    /// </summary>
    public class CombatArenaDirector : MonoBehaviour {
        [Header("Referencias de Escena")]
        [SerializeField] private Camera _combatCamera;
        [SerializeField] private Transform _enemySpawnPoint;
        [SerializeField] private Light _tensionLight;

        [Header("Visual del Enemigo")]
        [SerializeField] private GameObject _enemyVisualObject;
        [SerializeField] private Renderer _enemyRenderer;

        private Vector3 _originalCamPos;
        private Vector3 _originalEnemyPos;
        private Coroutine _shakeCoroutine;

        private void Awake() {
            if (_combatCamera != null) {
                _originalCamPos = _combatCamera.transform.localPosition;
            }
            if (_enemySpawnPoint != null) {
                _originalEnemyPos = _enemySpawnPoint.localPosition;
            }
        }

        private void OnEnable() {
            CombatManager.OnEnemyHit += HandleEnemyHit;
            CombatManager.OnPlayerDamaged += HandlePlayerDamaged;
        }

        private void OnDisable() {
            CombatManager.OnEnemyHit -= HandleEnemyHit;
            CombatManager.OnPlayerDamaged -= HandlePlayerDamaged;
        }

        private void Start() {
            SetupArena();
        }

        public void SetupArena() {
            var cm = CombatManager.Instance;
            if (cm == null || cm.CurrentEnemy == null) return;

            var enemyData = cm.CurrentEnemy;
            Debug.Log($"[CombatArena] Configurando arena de combate para: {enemyData.EnemyName}");

            // Configurar color/aspecto del modelo o sprite del enemigo
            if (_enemyRenderer != null) {
                // Color temático según el tipo de enemigo
                Color enemyColor = Color.red;
                if (enemyData.EnemyType == EnemyType.ZombiePoison) enemyColor = new Color(0.2f, 0.8f, 0.3f);
                else if (enemyData.EnemyType == EnemyType.ZombieFast) enemyColor = new Color(0.9f, 0.5f, 0.1f);
                else if (enemyData.EnemyType == EnemyType.Boss) enemyColor = new Color(0.5f, 0f, 0.5f);

                _enemyRenderer.material.color = enemyColor;
            }

            if (_tensionLight != null) {
                _tensionLight.color = new Color(1f, 0.2f, 0.2f);
                _tensionLight.intensity = 1.5f;
            }
        }

        private void HandleEnemyHit(HitOutcome outcome, int damage) {
            if (outcome == HitOutcome.Miss) return;

            // Sacudida del enemigo y parpadeo blanco/rojo
            if (_enemySpawnPoint != null) {
                StartCoroutine(ShakeObjectRoutine(_enemySpawnPoint, _originalEnemyPos, 0.2f, 0.15f));
            }
            if (_enemyRenderer != null) {
                StartCoroutine(FlashRendererRoutine(_enemyRenderer, outcome == HitOutcome.Critical ? Color.white : Color.red, 0.15f));
            }
        }

        private void HandlePlayerDamaged(int damage) {
            // Sacudida de cámara cuando el enemigo golpea al jugador
            if (_combatCamera != null) {
                if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
                _shakeCoroutine = StartCoroutine(ShakeObjectRoutine(_combatCamera.transform, _originalCamPos, 0.3f, 0.25f));
            }
        }

        private IEnumerator ShakeObjectRoutine(Transform target, Vector3 originalPos, float duration, float magnitude) {
            float elapsed = 0f;
            while (elapsed < duration) {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                target.localPosition = originalPos + new Vector3(x, y, 0f);
                elapsed += Time.deltaTime;
                yield return null;
            }
            target.localPosition = originalPos;
        }

        private IEnumerator FlashRendererRoutine(Renderer rend, Color flashColor, float duration) {
            Color origColor = rend.material.color;
            rend.material.color = flashColor;
            yield return new WaitForSeconds(duration);
            rend.material.color = origColor;
        }
    }
}

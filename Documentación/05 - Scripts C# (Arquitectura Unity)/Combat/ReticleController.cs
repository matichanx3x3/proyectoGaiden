using UnityEngine;
using UnityEngine.UI;

namespace Gaiden.Combat {
    /// <summary>
    /// Controlador visual de la aguja y las zonas del medidor en la UI de combate.
    /// </summary>
    public class ReticleController : MonoBehaviour {
        [Header("Elementos de Canvas")]
        [SerializeField] private RectTransform _gaugeRect;
        [SerializeField] private RectTransform _needleRect;
        [SerializeField] private RectTransform _hitZoneRect;
        [SerializeField] private RectTransform _critZoneRect;

        private void Update() {
            var cm = CombatManager.Instance;
            if (cm == null || cm.CurrentEnemy == null) return;

            // Ancho del contenedor
            float totalWidth = _gaugeRect.rect.width;
            float maxGbcGauge = 120f;

            // Escalar posiciones de GBC a píxeles de UI moderna
            float scale = totalWidth / maxGbcGauge;

            // Posicionar aguja
            float needleX = (cm.NeedlePosition - 60f) * scale;
            _needleRect.anchoredPosition = new Vector2(needleX, 0);

            // Ajustar zonas de impacto del enemigo activo
            float hitWidth = cm.CurrentEnemy.HitHalfWidth * 2f * scale;
            float critWidth = cm.CurrentEnemy.CritHalfWidth * 2f * scale;

            _hitZoneRect.sizeDelta = new Vector2(hitWidth, _hitZoneRect.sizeDelta.y);
            _critZoneRect.sizeDelta = new Vector2(critWidth, _critZoneRect.sizeDelta.y);
        }
    }
}

using UnityEngine;
using DG.Tweening;

namespace Game.Exploration {
    public enum BoxDestinationType {
        Blue, // Punto intermedio: se puede volver a mover la caja
        Red   // Punto final: la caja se bloquea permanentemente y habilita el peldaño para trepar
    }

    /// <summary>
    /// Marca un punto destino (socket/meta) para cajas empujables.
    /// Azul = Checkpoint intermedio (permite reubicar la caja).
    /// Rojo = Meta definitiva (bloquea la caja como plataforma escalable).
    /// </summary>
    public class BoxDestinationPoint : MonoBehaviour {
        [SerializeField] private BoxDestinationType _destinationType = BoxDestinationType.Blue;
        [SerializeField] private float _detectionRadius = 0.75f;
        [SerializeField] private AudioClip _socketSound;

        public BoxDestinationType DestinationType => _destinationType;
        public float DetectionRadius => _detectionRadius;

        public void Configure(BoxDestinationType type, float radius = 0.75f) {
            _destinationType = type;
            _detectionRadius = radius;
        }

        public void NotifyBoxEntered(PushableBox box) {
            transform.DOKill();
            transform.DOPunchScale(new Vector3(0.12f, 0.05f, 0.12f), 0.25f, 6, 0.5f);

            if (_socketSound != null) {
                var audioSrc = GetComponent<AudioSource>();
                if (audioSrc == null) audioSrc = gameObject.AddComponent<AudioSource>();
                audioSrc.PlayOneShot(_socketSound);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = (_destinationType == BoxDestinationType.Red) ? new Color(1f, 0.2f, 0.2f, 0.6f) : new Color(0.2f, 0.6f, 1f, 0.6f);
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}

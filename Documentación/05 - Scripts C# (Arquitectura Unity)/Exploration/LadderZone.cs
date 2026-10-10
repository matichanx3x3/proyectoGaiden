using UnityEngine;

namespace Game.Exploration {
    /// <summary>
    /// Zona de escaleras verticales. Al entrar el jugador en el trigger,
    /// activa el modo escalada en ExplorationController para permitir subir/bajar verticalmente.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class LadderZone : MonoBehaviour {
        private void Awake() {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<ExplorationController>(out var player)) {
                player.SetClimbingLadder(true);
                Debug.Log("[Ladder] Jugador inició escalada.");
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent<ExplorationController>(out var player)) {
                player.SetClimbingLadder(false);
                Debug.Log("[Ladder] Jugador finalizó escalada.");
            }
        }
    }
}

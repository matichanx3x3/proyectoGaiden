using System;
using UnityEngine;

namespace Gaiden.Core {
    public enum GaidenGameState {
        Starting    = 0,
        TitleScreen = 1,
        Exploration = 2,
        Combat      = 3,
        Inventory   = 4,
        GameOver    = 5,
        Victory     = 6
    }

    /// <summary>
    /// Gestor central de estados para el bucle de juego tipo Resident Evil Gaiden.
    /// Emite eventos desacoplados OnBeforeStateChanged y OnAfterStateChanged.
    /// </summary>
    public class GaidenGameManager : Singleton<GaidenGameManager> {
        public static event Action<GaidenGameState> OnBeforeStateChanged;
        public static event Action<GaidenGameState> OnAfterStateChanged;

        public GaidenGameState CurrentState { get; private set; }

        private void Start() {
            ChangeState(GaidenGameState.Starting);
        }

        public void ChangeState(GaidenGameState newState) {
            if (CurrentState == newState && newState != GaidenGameState.Starting) return;

            OnBeforeStateChanged?.Invoke(newState);
            CurrentState = newState;

            switch (newState) {
                case GaidenGameState.Starting:
                    HandleStarting();
                    break;
                case GaidenGameState.TitleScreen:
                    HandleTitleScreen();
                    break;
                case GaidenGameState.Exploration:
                    HandleExploration();
                    break;
                case GaidenGameState.Combat:
                    HandleCombat();
                    break;
                case GaidenGameState.Inventory:
                    HandleInventory();
                    break;
                case GaidenGameState.GameOver:
                    HandleGameOver();
                    break;
                case GaidenGameState.Victory:
                    HandleVictory();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(newState);
            Debug.Log($"[GaidenGameManager] Nuevo estado activo: {newState}");
        }

        private void HandleStarting() {
            ChangeState(GaidenGameState.Exploration);
        }

        private void HandleTitleScreen() {
            Time.timeScale = 1f;
        }

        private void HandleExploration() {
            Time.timeScale = 1f;
        }

        private void HandleCombat() {
            // El tiempo sigue corriendo para la oscilación del retículo y el turno del zombie
            Time.timeScale = 1f;
        }

        private void HandleInventory() {
            // Pausa el movimiento de exploración mientras se inspecciona el maletín
        }

        private void HandleGameOver() {
            Debug.Log("GAME OVER");
        }

        private void HandleVictory() {
            Debug.Log("ESCAPE COMPLETADO");
        }
    }
}

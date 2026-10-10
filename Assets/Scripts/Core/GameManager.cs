using System;
using UnityEngine;

namespace Game.Core {
    public enum GameState {
        Starting    = 0,
        TitleScreen = 1,
        Exploration = 2,
        Combat      = 3,
        Inventory   = 4,
        GameOver    = 5,
        Victory     = 6
    }

    /// <summary>
    /// Gestor central de estados para el bucle de juego.
    /// Emite eventos desacoplados OnBeforeStateChanged y OnAfterStateChanged.
    /// </summary>
    public class GameManager : Singleton<GameManager> {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;

        public GameState CurrentState { get; private set; } = GameState.Starting;

        private void Start() {
            ChangeState(GameState.Exploration);
        }

        public void ChangeState(GameState newState) {
            if (CurrentState == newState && newState != GameState.Starting) return;

            OnBeforeStateChanged?.Invoke(newState);
            CurrentState = newState;

            switch (newState) {
                case GameState.Starting:
                    HandleStarting();
                    break;
                case GameState.TitleScreen:
                    HandleTitleScreen();
                    break;
                case GameState.Exploration:
                    HandleExploration();
                    break;
                case GameState.Combat:
                    HandleCombat();
                    break;
                case GameState.Inventory:
                    HandleInventory();
                    break;
                case GameState.GameOver:
                    HandleGameOver();
                    break;
                case GameState.Victory:
                    HandleVictory();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(newState);
            Debug.Log($"[GameManager] Nuevo estado activo: {newState}");
        }

        private void HandleStarting() {
            ChangeState(GameState.Exploration);
        }

        private void HandleTitleScreen() {
            Time.timeScale = 1f;
        }

        private void HandleExploration() {
            Time.timeScale = 1f;
        }

        private void HandleCombat() {
            Time.timeScale = 1f;
        }

        private void HandleInventory() {
        }

        private void HandleGameOver() {
            Debug.Log("[GameManager] GAME OVER");
        }

        private void HandleVictory() {
            Debug.Log("[GameManager] ESCAPE / VICTORIA COMPLETADA");
        }
    }
}

using UnityEngine;

namespace Gaiden.Core {
    /// <summary>
    /// Instancia estática que sobrescribe la versión previa al crearse una nueva.
    /// Ideal para gestores atados al ciclo de vida de una escena.
    /// </summary>
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
        public static T Instance { get; private set; }

        protected virtual void Awake() => Instance = this as T;

        protected virtual void OnApplicationQuit() {
            Instance = null;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Singleton estándar. Destruye cualquier nueva copia duplicada que se cree.
    /// </summary>
    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
        protected override void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            base.Awake();
        }
    }

    /// <summary>
    /// Singleton persistente. Sobrevive a la carga de escenas (DontDestroyOnLoad).
    /// </summary>
    public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour {
        protected override void Awake() {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}

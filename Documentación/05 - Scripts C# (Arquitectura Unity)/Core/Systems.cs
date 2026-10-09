using UnityEngine;

namespace Gaiden.Core {
    /// <summary>
    /// Contenedor raíz persistente de sistemas globales.
    /// Utiliza auto-inicialización antes de la carga de la primera escena.
    /// </summary>
    public class Systems : PersistentSingleton<Systems> {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitializeBootstrap() {
            if (Instance == null) {
                var prefab = Resources.Load<GameObject>("Systems");
                if (prefab != null) {
                    var instance = Instantiate(prefab);
                    instance.name = "[SYSTEMS]";
                } else {
                    var go = new GameObject("[SYSTEMS]");
                    go.AddComponent<Systems>();
                }
            }
        }
    }
}

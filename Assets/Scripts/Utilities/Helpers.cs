using UnityEngine;

namespace Game.Utilities {
    public static class Helpers {
        public static void DestroyChildren(this Transform t) {
            foreach (Transform child in t) {
                Object.Destroy(child.gameObject);
            }
        }

        public static Vector3 SnapToGrid(this Vector3 position, float gridSize = 1f) {
            return new Vector3(
                Mathf.Round(position.x / gridSize) * gridSize,
                position.y,
                Mathf.Round(position.z / gridSize) * gridSize
            );
        }

        public static float CalculateReticlePosition(float time, float speed, float gaugeMax = 120f) {
            return Mathf.PingPong(time * speed, gaugeMax);
        }
    }
}

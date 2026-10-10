using UnityEngine;

namespace Game.Units {
    public enum EnemyType {
        ZombieStandard = 0,
        ZombieFast     = 1,
        ZombieArmored  = 2,
        ZombiePoison   = 3,
        CreatureMiniBoss = 4,
        Boss           = 5,
        Custom         = 99
    }

    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Units/Enemy Shell")]
    public class ScriptableEnemy : ScriptableObject {
        public EnemyType EnemyType = EnemyType.ZombieStandard;
        public string EnemyName = "Enemy Shell";
        public int MaxHealth = 40;
        public int AttackPower = 15;

        [Header("Retículo de Combate (Calibración)")]
        [Tooltip("Semi-ancho de la zona de impacto normal sobre 60px")]
        [Range(4, 30)] public int HitHalfWidth = 18;

        [Tooltip("Semi-ancho de la zona de crítico en el centro")]
        [Range(1, 10)] public int CritHalfWidth = 4;

        [Tooltip("Frames o intervalo de ataque del enemigo en combate")]
        [Range(30, 300)] public int AttackIntervalFrames = 180;

        [Header("Visuales")]
        public Sprite BattleSprite;
        public GameObject OverworldPrefab;
    }
}

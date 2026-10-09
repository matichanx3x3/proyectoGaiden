using UnityEngine;

namespace Gaiden.Units {
    public enum EnemyType {
        ZombieMale = 0,
        ZombieFemale = 1,
        ZombieArmed = 2,
        ZombiePoison = 3,
        Cerberus = 4,
        LeechCreature = 5,
        TyrantBow = 6
    }

    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Gaiden/Units/Enemy")]
    public class ScriptableEnemy : ScriptableObject {
        public EnemyType EnemyType;
        public string EnemyName;
        public int MaxHealth;
        public int AttackPower;

        [Header("Retículo de Combate")]
        [Range(4, 30)] public int HitHalfWidth = 18;
        [Range(1, 8)]  public int CritHalfWidth = 4;
        [Range(60, 300)] public int AttackIntervalFrames = 180;

        public Sprite BattleSprite;
        public GameObject OverworldPrefab;
    }
}

using UnityEngine;

namespace Gaiden.Weapons {
    public enum WeaponType {
        Knife = 0,
        Handgun = 1,
        Shotgun = 2,
        GrenadeLauncher = 3,
        RocketLauncher = 4
    }

    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Gaiden/Items/Weapon")]
    public class ScriptableWeapon : ScriptableObject {
        public WeaponType WeaponType;
        public string WeaponName;
        public int BaseDamage;
        public int CritMultiplier = 2;
        public int MaxAmmo = 99;
        public AudioClip FireSound;
        public Sprite Icon;
    }
}

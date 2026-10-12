using UnityEngine;

namespace Game.Weapons {
    public enum WeaponType {
        MeleeKnife      = 0,
        Handgun         = 1,
        Shotgun         = 2,
        Rifle           = 3,
        HeavyExplosive  = 4
    }

    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Items/Weapon")]
    public class ScriptableWeapon : ScriptableObject {
        public WeaponType WeaponType = WeaponType.Handgun;
        public string WeaponName = "Handgun";
        public int BaseDamage = 10;
        public int CritMultiplier = 2;
        public int MaxAmmo = 99;
        public AudioClip FireSound;
        public Sprite Icon;
    }
}

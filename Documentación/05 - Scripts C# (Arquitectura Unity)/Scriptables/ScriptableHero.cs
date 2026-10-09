using UnityEngine;

namespace Gaiden.Units {
    [CreateAssetMenu(fileName = "NewHero", menuName = "Gaiden/Units/Hero")]
    public class ScriptableHero : ScriptableObject {
        public HeroId HeroId;
        public string CharacterName;
        public int MaxHealth;
        public Sprite Portrait;
        public GameObject Prefab;
    }
}

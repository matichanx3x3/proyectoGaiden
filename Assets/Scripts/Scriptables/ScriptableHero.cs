using UnityEngine;

namespace Game.Units {
    public enum HeroId {
        Hero_01 = 0, // Placeholder / Cáscara 1 (Ej: Protagonista)
        Hero_02 = 1, // Placeholder / Cáscara 2 (Ej: Compañero A)
        Hero_03 = 2, // Placeholder / Cáscara 3 (Ej: Compañero B)
        Custom  = 99
    }

    /// <summary>
    /// Cáscara de datos desacoplada para definir o personalizar personajes jugables.
    /// </summary>
    [CreateAssetMenu(fileName = "NewHero", menuName = "Game/Units/Hero Shell")]
    public class ScriptableHero : ScriptableObject {
        public HeroId HeroId = HeroId.Hero_01;
        public string CharacterName = "Hero Shell";
        [TextArea] public string BioOrDescription = "Personaje por definir.";
        public int MaxHealth = 100;
        public float MoveSpeed = 3.5f;
        public float SprintSpeed = 5.5f;
        public Sprite Portrait;
        public GameObject Prefab;
    }
}

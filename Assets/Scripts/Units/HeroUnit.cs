using UnityEngine;

namespace Game.Units {
    /// <summary>
    /// Componente de la cáscara del personaje activo en el mundo de exploración.
    /// </summary>
    public class HeroUnit : UnitBase {
        public HeroId HeroId { get; private set; }
        private ScriptableHero _data;

        public void Initialize(ScriptableHero data) {
            _data = data;
            HeroId = data.HeroId;
            SetStats(new UnitStats {
                Health = data.MaxHealth,
                MaxHealth = data.MaxHealth,
                AttackPower = 0
            });
        }
    }
}

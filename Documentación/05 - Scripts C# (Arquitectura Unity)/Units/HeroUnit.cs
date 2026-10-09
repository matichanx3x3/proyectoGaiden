using UnityEngine;

namespace Gaiden.Units {
    /// <summary>
    /// Componente del protagonista en el mundo de exploración.
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

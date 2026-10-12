using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Units {
    [Serializable]
    public class PartyMember {
        public HeroId Id = HeroId.Hero_01;
        public string Name = "Hero Shell";
        public int CurrentHp = 100;
        public int MaxHp = 100;
        public bool IsInParty = true;
        public bool IsPoisoned = false;

        public bool IsAlive => CurrentHp > 0;

        public void TakeDamage(int dmg) => CurrentHp = Mathf.Max(0, CurrentHp - dmg);
        public void Heal(int amount) => CurrentHp = Mathf.Min(MaxHp, CurrentHp + amount);
    }

    /// <summary>
    /// Gestiona las cáscaras de los personajes del grupo (Party), sus puntos de vida, estados y rotación.
    /// Diseñado para ser modular y extensible cuando los personajes finales sean definidos.
    /// </summary>
    public class PartyManager : Singleton<PartyManager> {
        public static event Action<PartyMember> OnActiveHeroChanged;
        public static event Action<PartyMember> OnHeroDamaged;

        [Header("Miembros del Grupo (Cáscaras Configurables)")]
        [SerializeField] private List<PartyMember> _members = new List<PartyMember>();

        public int ActiveIndex { get; private set; } = 0;
        public PartyMember ActiveHero => (_members != null && _members.Count > ActiveIndex) ? _members[ActiveIndex] : null;
        public IReadOnlyList<PartyMember> AllMembers => _members;

        protected override void Awake() {
            base.Awake();
            if (_members == null || _members.Count == 0) {
                InitializeDefaultShells();
            }
        }

        public void InitializeDefaultShells() {
            _members = new List<PartyMember> {
                new PartyMember { Id = HeroId.Hero_01, Name = "Protagonist Shell", CurrentHp = 100, MaxHp = 100, IsInParty = true },
                new PartyMember { Id = HeroId.Hero_02, Name = "Companion A Shell", CurrentHp = 100, MaxHp = 100, IsInParty = false },
                new PartyMember { Id = HeroId.Hero_03, Name = "Companion B Shell", CurrentHp = 120, MaxHp = 120, IsInParty = false }
            };
        }

        public void Recruit(HeroId id) {
            var m = _members.Find(x => x.Id == id);
            if (m != null) m.IsInParty = true;
        }

        public void CycleNextHero() {
            if (_members == null || _members.Count == 0) return;

            for (int i = 1; i < _members.Count; i++) {
                int next = (ActiveIndex + i) % _members.Count;
                if (_members[next].IsInParty && _members[next].IsAlive) {
                    ActiveIndex = next;
                    OnActiveHeroChanged?.Invoke(ActiveHero);
                    return;
                }
            }
        }

        public void DamageActiveHero(int amount) {
            if (ActiveHero == null) return;

            ActiveHero.TakeDamage(amount);
            OnHeroDamaged?.Invoke(ActiveHero);

            if (!ActiveHero.IsAlive) {
                // Auto-conmutación a un personaje con vida
                bool aliveFound = false;
                for (int i = 0; i < _members.Count; i++) {
                    if (_members[i].IsInParty && _members[i].IsAlive) {
                        ActiveIndex = i;
                        OnActiveHeroChanged?.Invoke(ActiveHero);
                        aliveFound = true;
                        break;
                    }
                }
                if (!aliveFound && GameManager.Instance != null) {
                    GameManager.Instance.ChangeState(GameState.GameOver);
                }
            }
        }

        public void CurePoison() {
            if (ActiveHero != null) ActiveHero.IsPoisoned = false;
        }
    }
}

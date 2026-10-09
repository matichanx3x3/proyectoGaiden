using System;
using System.Collections.Generic;
using UnityEngine;
using Gaiden.Core;

namespace Gaiden.Units {
    public enum HeroId { Barry = 0, Leon = 1, Lucia = 2 }

    [Serializable]
    public class PartyMember {
        public HeroId Id;
        public string Name;
        public int CurrentHp;
        public int MaxHp;
        public bool IsInParty;
        public bool IsPoisoned;

        public bool IsAlive => CurrentHp > 0;

        public void TakeDamage(int dmg) => CurrentHp = Mathf.Max(0, CurrentHp - dmg);
        public void Heal(int amount) => CurrentHp = Mathf.Min(MaxHp, CurrentHp + amount);
    }

    /// <summary>
    /// Gestiona a Barry Burton, Leon S. Kennedy y Lucia, sus puntos de vida y miembros vivos.
    /// </summary>
    public class PartyManager : Singleton<PartyManager> {
        public static event Action<PartyMember> OnActiveHeroChanged;
        public static event Action<PartyMember> OnHeroDamaged;

        [SerializeField] private List<PartyMember> _members = new List<PartyMember>();
        public int ActiveIndex { get; private set; } = 0;

        public PartyMember ActiveHero => _members[ActiveIndex];

        protected override void Awake() {
            base.Awake();
            InitializeParty();
        }

        private void InitializeParty() {
            _members.Clear();
            _members.Add(new PartyMember { Id = HeroId.Barry, Name = "Barry", CurrentHp = 100, MaxHp = 100, IsInParty = true });
            _members.Add(new PartyMember { Id = HeroId.Leon, Name = "Leon", CurrentHp = 100, MaxHp = 100, IsInParty = false });
            _members.Add(new PartyMember { Id = HeroId.Lucia, Name = "Lucia", CurrentHp = 120, MaxHp = 120, IsInParty = false });
        }

        public void Recruit(HeroId id) {
            var m = _members.Find(x => x.Id == id);
            if (m != null) m.IsInParty = true;
        }

        public void CycleNextHero() {
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
                if (!aliveFound) {
                    GaidenGameManager.Instance.ChangeState(GaidenGameState.GameOver);
                }
            }
        }

        public void CurePoison() => ActiveHero.IsPoisoned = false;
    }
}

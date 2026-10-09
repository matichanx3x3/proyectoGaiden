using Gaiden.Combat;
using Gaiden.Exploration;

namespace Gaiden.Interfaces {
    public interface IDamageable {
        int CurrentHp { get; }
        bool IsDead { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
    }

    public interface IInteractable {
        void Interact(ExplorationController player);
    }

    public interface ICombatTarget {
        int TargetCenter { get; }
        int HitHalfWidth { get; }
        int CritHalfWidth { get; }
        void OnHitReceived(HitOutcome outcome, int damage);
    }
}

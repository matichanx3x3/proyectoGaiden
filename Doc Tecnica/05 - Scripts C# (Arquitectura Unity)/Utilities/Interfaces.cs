using Game.Combat;
using Game.Exploration;

namespace Game.Interfaces {
    public interface IDamageable {
        int CurrentHp { get; }
        int MaxHp { get; }
        bool IsDead { get; }
        void TakeDamage(int amount);
        void Heal(int amount);
    }

    public interface IInteractable {
        void Interact(ExplorationController player);
        string GetInteractionPrompt();
    }

    public interface ICombatTarget {
        int TargetCenter { get; }
        int HitHalfWidth { get; }
        int CritHalfWidth { get; }
        void OnHitReceived(HitOutcome outcome, int damage);
    }
}

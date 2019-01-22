using MtgEngine.Common.Cards;

namespace MtgEngine.Common
{
    public interface IDamageable
    {
        void TakeDamage(int amount, Card source);

        bool IsDead { get; }
    }
}

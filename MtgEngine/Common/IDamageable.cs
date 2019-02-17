using MtgEngine.Common.Cards;

namespace MtgEngine.Common
{
    public delegate void TookDamageEventHandler(IDamageable self, Card source, int amount);

    public interface IDamageable
    {
        void TakeDamage(int amount, Card source);

        bool IsDead { get; }

        /// <summary>
        /// Fire this event when TakeDamage is called
        /// </summary>
        event TookDamageEventHandler TookDamage;
    }
}

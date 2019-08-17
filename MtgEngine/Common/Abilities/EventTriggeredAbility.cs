using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Abilities
{
    public abstract class EventTriggeredAbility : TriggeredAbility
    {
        protected EventTriggeredAbility(Card source, string text) : base(source, text)
        {
        }

        public virtual void PermanentGotCounter(Game game, Card card, IResolvable source, CounterType counterType, int count)
        {
        }

        public virtual void PlayerGotCounter(Game game, Player player, CounterType counterType, int count)
        {
        }

        public virtual void PlayerTookDamage(Game game, Player player, Card source, int damageReceived)
        {
        }

        public virtual void PlayerLostLife(Game game, Player player, IResolvable source, int lifeLost)
        {
        }

        public virtual void CreatureTookDamage(Game game, Card creature, Card source, int damageReceived)
        {
        }

        public virtual void CardHasChangedZones(Game game, Card card, Enums.Zone previousZone, Enums.Zone currentZone)
        {
        }

        public virtual void PlayerDrewCards(Game game, Player player, int cardsDrawn)
        {
        }

        public virtual void AttackerDeclared(Game game, Card attacker, Player defendingPlayer)
        {
        }

        public virtual void BlockerDeclared(Game game, Card attacker, Card blocker)
        {
        }
    }
}

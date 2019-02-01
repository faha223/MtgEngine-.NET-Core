using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Abilities
{
    public abstract class EventTriggeredAbility : TriggeredAbility
    {
        protected EventTriggeredAbility(PermanentCard source, string text) : base(source, text)
        {
        }

        public virtual void PermanentEnteredBattlefield(Game game, PermanentCard card)
        {
        }

        public virtual void PermanentGotCounter(Game game, PermanentCard card, CounterType counterType, int count)
        {
        }

        public virtual void PlayerGotCounter(Game game, Player player, CounterType counterType, int count)
        {
        }

        public virtual void PermanentChangedZones(Game game, PermanentCard card, Enums.Zone previousZone, Enums.Zone currentZone)
        {
        }
    }
}

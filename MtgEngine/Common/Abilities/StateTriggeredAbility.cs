using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Abilities
{
    public abstract class StateTriggeredAbility : TriggeredAbility
    {
        public abstract bool CheckState(Game game);

        protected StateTriggeredAbility(PermanentCard source, string text) : base(source, text)
        {
        }
    }
}

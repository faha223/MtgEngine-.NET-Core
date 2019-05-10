using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Abilities
{
    public abstract class TriggeredAbility : Ability
    {
        protected TriggeredAbility(Card source, string text) : base(source, text)
        {
        }
    }
}

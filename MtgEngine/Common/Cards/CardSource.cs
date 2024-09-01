using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public abstract class CardSource
    {
        public abstract Card GetCard(Player owner);

        protected MtgCardAttribute CardAttrs => MtgCardAttribute.GetAttribute(GetType());
    }
}

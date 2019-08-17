using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Enchantments
{
    [MtgCard("Blessing", "LEA", "", "", "{W}: Target creature gets +1/+1 until end of turn.")]
    public class Blessing : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, null, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            // TODO: {W}: Target creature gets +1/+1 until end of turn

            return card;
        }
    }
}

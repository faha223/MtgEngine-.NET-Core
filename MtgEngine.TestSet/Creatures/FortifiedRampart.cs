using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Fortified Rampart", "TestSet", "", "", "Defender", "The refuge's defenses allow new recruits to see lesser Eldrazi up close, steeling their stomachs for what's to come.")]
    public class FortifiedRampart : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Wall" }, false, 0, 6, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}{W}");
            card.AddStaticAbility(StaticAbility.Defender);

            return card;
        }
    }
}

using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    public class BogWraith : CardSource
    {
        [MtgCard("Bog Wraith", "LEA", "", "", "Swampwalk", "'Twas in the bogs of Cannelbrae My mate did meet an early grave. 'Twas nothing left for us to save In the peat-filled bogs of Cannelbrae.")]
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Wraith" }, false, 3, 3, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}{B}");

            card.AddStaticAbility(StaticAbility.Swampwalk);

            return card;
        }
    }
}

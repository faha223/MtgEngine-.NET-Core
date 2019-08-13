using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Grizzly Bears", "TestSet", "", "", null, "\"We cannot forget that among all of Dominaria's wonders, a system of life exists, with prey and predators that will never fight wars nor vie for ancient power.\"\n-Jorael, empress of beasts")]
    public class GrizzlyBears : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new CardType[] { CardType.Creature }, new string[] { "Bear" }, false, 2, 2, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}{G}");

            return card;
        }
    }
}

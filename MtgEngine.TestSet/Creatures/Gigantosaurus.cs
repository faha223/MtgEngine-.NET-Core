using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Gigantosaurus", "TestSet", "", "", null, "\"Each tooth is the length of a horse, and new ones grow in every sixteen days.\nLet\'s get a closer look!\"\n-Vivien Reid")]
    public class Gigantosaurus : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Dinosaur" }, false, 10, 10, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{G}{G}{G}{G}{G}");

            return card;
        }
    }
}

using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Rustwing Falcon", "TestSet", "", "", Text ="Flying", FlavorText = "Native to wide prairies and scrublands, falcons occasionally roost in dragon skeletons.")]
    public class RustwingFalcon : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Bird" }, false, 1, 2, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{W}");
            card.StaticAbilities.Add(StaticAbility.Flying);

            return card;
        }
    }
}

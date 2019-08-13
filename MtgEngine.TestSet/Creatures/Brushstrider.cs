using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Brushstrider", "TestSet", "", "", "Vigilance", "Magistrate Ludy agreed to designate land for the brushstriders only after several broken windows and dozens of missing blini-cakes.")]
    public class Brushstrider : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Beast" }, false, 3, 1, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{1}{G}");
            card.StaticAbilities.Add(StaticAbility.Vigilance);

            return card;
        }
    }
}

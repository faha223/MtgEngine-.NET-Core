using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Giant Spider", "TestSet", "", "", "Reach (This creature can block creatures with flying.)", "\"After everything I’ve survived, it’s hard to be frightened by anything anymore.\"\n-Vivien Reid")]
    public class GiantSpider : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Spider" }, false, 2, 4, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}{G}");
            card.StaticAbilities.Add(StaticAbility.Reach);

            return card;
        }
    }
}

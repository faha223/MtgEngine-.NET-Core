using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Air Elemental", "LEA", "", "", FlavorText = "These spirits of the air are winsome and wild, and cannot be truly contained. Only marginally intelligent, they often substitute whimsy for strategy, delighting in mischief and mayhem.")]
    public class AirElemental : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Elemental" }, false, 4, 4, false, false);
            card._attrs = CardAttrs;

            card.Cost = ManaCost.Parse(card, "{3}{U}{U}");
            card.AddStaticAbility(StaticAbility.Flying);

            return card;
        }
    }
}

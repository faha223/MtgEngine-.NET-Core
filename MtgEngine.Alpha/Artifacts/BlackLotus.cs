using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    [MtgCard("Black Lotus", "LEA", "", "", Text = "{T}, Sacrifice Black Lotus: Add three mana of any one color.")]
    public class BlackLotus : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact }, null, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.AddAbility(new ManaAbility(card, new AggregateCost(card, new TapCost(card), new SacrificeSourceCost(card)), new ManaAmount(3, ManaColor.White), "{T}, Sacrifice Black Lotus: Add {W}{W}{W}"));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, new TapCost(card), new SacrificeSourceCost(card)), new ManaAmount(3, ManaColor.Blue), "{T}, Sacrifice Black Lotus: Add {U}{U}{U}"));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, new TapCost(card), new SacrificeSourceCost(card)), new ManaAmount(3, ManaColor.Black), "{T}, Sacrifice Black Lotus: Add {B}{B}{B}"));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, new TapCost(card), new SacrificeSourceCost(card)), new ManaAmount(3, ManaColor.Red), "{T}, Sacrifice Black Lotus: Add {R}{R}{R}"));
            card.AddAbility(new ManaAbility(card, new AggregateCost(card, new TapCost(card), new SacrificeSourceCost(card)), new ManaAmount(3, ManaColor.Green), "{T}, Sacrifice Black Lotus: Add {G}{G}{G}"));

            return card;
        }
    }
}

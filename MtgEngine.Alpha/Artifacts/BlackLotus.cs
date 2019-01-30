using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    [MtgCard("Black Lotus", "LEA", "", "", Text = "{T}, Sacrifice Black Lotus: Add three mana of any one color.")]
    public class BlackLotus : ArtifactCard
    {
        public BlackLotus(Player owner) : base(owner, true, null, new[] { CardType.Artifact }, null, false, false)
        {
            Abilities.Add(new ManaAbility(this, new AggregateCost(this, new TapCost(this), new SacrificeSourceCost(this)), new ManaAmount(3, ManaColor.White), "{T}, Sacrifice Black Lotus: Add {W}{W}{W}"));
            Abilities.Add(new ManaAbility(this, new AggregateCost(this, new TapCost(this), new SacrificeSourceCost(this)), new ManaAmount(3, ManaColor.Blue), "{T}, Sacrifice Black Lotus: Add {U}{U}{U}"));
            Abilities.Add(new ManaAbility(this, new AggregateCost(this, new TapCost(this), new SacrificeSourceCost(this)), new ManaAmount(3, ManaColor.Black), "{T}, Sacrifice Black Lotus: Add {B}{B}{B}"));
            Abilities.Add(new ManaAbility(this, new AggregateCost(this, new TapCost(this), new SacrificeSourceCost(this)), new ManaAmount(3, ManaColor.Red), "{T}, Sacrifice Black Lotus: Add {R}{R}{R}"));
            Abilities.Add(new ManaAbility(this, new AggregateCost(this, new TapCost(this), new SacrificeSourceCost(this)), new ManaAmount(3, ManaColor.Green), "{T}, Sacrifice Black Lotus: Add {G}{G}{G}"));
        }
    }
}

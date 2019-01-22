using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Taiga", "LEA", "", "", "({T}: Add {R} or {G})")]
    public class Taiga : LandCard
    {
        public Taiga(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Mountain", "Forest" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {R}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {G}"));
        }
    }
}

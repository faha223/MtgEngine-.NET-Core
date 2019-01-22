using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Savannah", "LEA", "", "", "({T}: Add {G} or {W})")]
    public class Savannah : LandCard
    {
        public Savannah(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Forest", "Plains" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {G}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {W}"));
        }
    }
}

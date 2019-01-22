using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Tundra", "LEA", "", "", "({T}: Add {W} or {U})")]
    public class Tundra : LandCard
    {
        public Tundra(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Plains", "Island" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {W}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {U}"));
        }
    }
}

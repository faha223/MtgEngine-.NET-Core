using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Badlands", "LEA", "", "", "({T}: Add {B} or {R})")]
    public class Badlands : Card
    {
        public Badlands(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Swamp", "Mountain" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Black), "{T}: Add {B}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {R}"));
        }
    }
}

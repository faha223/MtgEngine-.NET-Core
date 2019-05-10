using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Underground Sea", "LEA", "", "", "({T}: Add {U} or {B})")]
    public class UndergroundSea : Card
    {
        public UndergroundSea(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Island", "Swamp" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {B}"));
        }
    }
}

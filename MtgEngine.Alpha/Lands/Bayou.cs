using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Lands
{
    [MtgCard("Bayou", "LEA", "", "", "({T}: Add {B} or {G})")]
    public class Bayou : Card
    {
        public Bayou(Player owner) : base(owner, new[] { CardType.Land }, new[] { "Swamp", "Forest" }, false, false, false)
        {
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Black), "{T}: Add {B}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Green), "{T}: Add {G}"));
        }
    }
}

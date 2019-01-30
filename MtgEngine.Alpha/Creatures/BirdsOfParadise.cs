using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Birds of Paradise", "LEA", "", "")]
    public class BirdsOfParadise : PermanentCard
    {
        public BirdsOfParadise(Player owner) : base(owner, true, null, new[] { CardType.Creature }, new[] { "Bird" }, false, 0, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{G}");

            StaticAbilities.Add(StaticAbility.Flying);

            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.White), "{T}: Add {W}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Blue), "{T}: Add {U}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Black), "{T}: Add {B}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Red), "{T}: Add {R}"));
            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Green), "{T}: Add {G}"));
        }
    }
}

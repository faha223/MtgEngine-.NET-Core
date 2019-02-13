using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Plague Myr", "TestSet", "", "", Text = "Infect (This creature deals damage to creatures in the form of -1/-1 counters and to players in the form of poison counters.){T}: Add {C}.", FlavorText = "They watch for a new master, one more sinister than the last.")]
    public class PlagueMyr : PermanentCard
    {
        public PlagueMyr(Player owner) : base(owner, true, null, new[] { CardType.Artifact, CardType.Creature }, new[] { "Myr" }, false, 1, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{2}");

            StaticAbilities.Add(StaticAbility.Infect);

            Abilities.Add(new ManaAbility(this, new TapCost(this), new Common.Mana.ManaAmount(1, ManaColor.Colorless), "{T}: Add {C}."));
        }
    }
}

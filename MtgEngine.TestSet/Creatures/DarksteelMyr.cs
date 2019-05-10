using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Darksteel Myr", "TestSet", "", "", "Indestructible (Damage and effects that say \"destroy\" don't destroy this creature. If its toughness is 0 or less, it's still put into its owner's graveyard.)", "")]
    public class DarksteelMyr : Card
    {
        public DarksteelMyr(Player owner) : base(owner, new[] { CardType.Artifact, CardType.Creature }, new[] { "Myr" }, false, 0, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{3}");
            StaticAbilities.Add(StaticAbility.Indestructible);
        }
    }
}

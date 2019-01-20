using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Rustwing Falcon", "TestSet", "", "", Text ="Flying", FlavorText = "Native to wide prairies and scrublands, falcons occasionally roost in dragon skeletons.")]
    public class RustwingFalcon : PermanentCard
    {
        public RustwingFalcon(Player owner) : base(owner, true, null, new[] { CardType.Creature }, new[] { "Bird" }, false, 1, 2, false, false)
        {
            Cost = ManaCost.Parse(this, "{W}");
            StaticAbilities.Add(StaticAbility.Flying);
        }
    }
}

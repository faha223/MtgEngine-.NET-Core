using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Fortified Rampart", "TestSet", "", "", "Defender", "The refuge's defenses allow new recruits to see lesser Eldrazi up close, steeling their stomachs for what's to come.")]
    public class FortifiedRampart : Card
    {
        public FortifiedRampart(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Wall" }, false, 0, 6, false, false)
        {
            Cost = ManaCost.Parse(this, "{1}{W}");
            StaticAbilities.Add(StaticAbility.Defender);
        }
    }
}

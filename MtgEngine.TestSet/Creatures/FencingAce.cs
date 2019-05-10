using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Fencing Ace", "TestSet", "", "", "Double strike (This creature deals both first-strike and regular combat damage.)", "His prowess gives the guildless hope that they can hold out against tyranny.")]
    public class FencingAce : Card
    {
        public FencingAce(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Human", "Soldier" }, false, 1, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{1}{W}");
            StaticAbilities.Add(StaticAbility.DoubleStrike);
        }
    }
}

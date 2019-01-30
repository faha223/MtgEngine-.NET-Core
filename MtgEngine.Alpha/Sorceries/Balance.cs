using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Sorceries
{
    [MtgCard("Balance", "LEA", "", "", Text = "Each player chooses a number of lands they control equal to the number of lands controlled by the player who controls the fewest, then sacrifices the rest. Players discard cards and sacrifice creatures the same way.")]
    public class Balance : SpellCard
    {
        public Balance(Player owner) : base(owner, null, new[] { CardType.Sorcery }, null, false)
        {
            Cost = ManaCost.Parse(this, "{1}{W}");
        }

        public override void OnResolve(Game game)
        {
            // TODO: Each player chooses a number of lands they control equal to the number of lands controlled by the player who controls the fewest, and sacrifices the rest

            // TODO: Each player chooses a number of cards in their hand they control equal to the number of cards in the hand of the the player who holds the fewest, and discards the rest

            // TODO: Each player chooses a number of creatures they control equal to the number of creatures controlled by the player who controls the fewest, and sacrifices the rest. They cannot be regenerated.
        }
    }
}

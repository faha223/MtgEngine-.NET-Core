using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Players.Gameplay
{
    public class BlockerDeclaration
    {
        public CreatureCard Attacker { get; set; }
        public CreatureCard Blocker { get; set; }
    }
}

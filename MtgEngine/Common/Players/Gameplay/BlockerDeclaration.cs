using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Players.Gameplay
{
    public class BlockerDeclaration
    {
        public Card Attacker { get; set; }
        public Card Blocker { get; set; }
    }
}

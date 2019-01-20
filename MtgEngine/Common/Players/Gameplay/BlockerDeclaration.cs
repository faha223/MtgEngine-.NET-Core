using MtgEngine.Common.Cards;

namespace MtgEngine.Common.Players.Gameplay
{
    public class BlockerDeclaration
    {
        public PermanentCard Attacker { get; set; }
        public PermanentCard Blocker { get; set; }
    }
}

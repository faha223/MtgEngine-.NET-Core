using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards
{
    public sealed partial class Card
    {
        Card CreateATokenCopy(Player owner)
        {
            var copy = new Card(owner, this, true);
            return copy;
        }
    }
}

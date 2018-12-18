using MtgEngine.Common.Cards.BasicLands;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    public class BasicSnowLand : BasicLandCard
    {
        public BasicSnowLand(Player owner, ManaColor color, CardType[] types, string[] subtypes) : base(owner, color, types, subtypes, true)
        {
        }
    }
}

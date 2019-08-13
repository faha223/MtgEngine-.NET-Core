using MtgEngine.Common.Cards.BasicLands;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicSnowLands
{
    public abstract class BasicSnowLandSource : BasicLandCardSource
    {
        public Card GetBasicSnowLand(Player owner, ManaColor color, CardType[] types, string[] subtypes) 
        {
            return GetBasicLandCard(owner, color, types, subtypes, true);
        }
    }
}

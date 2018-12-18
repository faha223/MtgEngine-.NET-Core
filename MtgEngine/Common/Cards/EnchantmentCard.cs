using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract class EnchantmentCard : PermanentCard
    {
        public EnchantmentCard(Player owner, bool usesStack, Cost cost, CardType[] types, string[] subtypes, bool isLegendary, bool isSnow) :
            base(owner, usesStack, cost, types, subtypes, false, isLegendary, isSnow)
        {
        }
    }
}

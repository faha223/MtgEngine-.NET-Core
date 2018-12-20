using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    public abstract class BasicLandCard : LandCard
    {
        protected BasicLandCard(Player owner, ManaColor color, CardType[] types, string[] subtypes, bool isSnow) : 
            base(owner, types, subtypes, true, false, isSnow)
        {
            // Basic Lands tap for 1 mana of their specific color
            var cost = new TapCost(this);
            var manaGenerated = new ManaAmount(1, color);
            Abilities.Add(new ManaAbility(this, cost, manaGenerated, $"{cost}: Add {manaGenerated}"));
        }
    }
}

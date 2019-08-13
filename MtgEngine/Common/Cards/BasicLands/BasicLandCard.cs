using MtgEngine.Common.Abilities;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Cards.BasicLands
{
    public abstract class BasicLandCardSource : CardSource
    {
        protected Card GetBasicLandCard(Player owner, ManaColor color, CardType[] types, string[] subtypes, bool isSnow)
        {
            var card = new Card(owner, types, subtypes, true, 0, 0, false, isSnow);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
            // Basic Lands tap for 1 mana of their specific color
            var cost = new TapCost(card);
            var manaGenerated = new ManaAmount(1, color);
            card.Abilities.Add(new ManaAbility(card, cost, manaGenerated, $"{cost}: Add {manaGenerated}"));

            return card;
        }
    }
}

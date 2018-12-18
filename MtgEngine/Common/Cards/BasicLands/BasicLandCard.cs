using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards.BasicLands
{
    public abstract class BasicLandCard : LandCard
    {
        ManaColor _color;

        protected BasicLandCard(Player owner, ManaColor color, CardType[] types, string[] subtypes, bool isSnow) : 
            base(owner, types, subtypes, true, false, isSnow)
        {
            _color = color;

            // TODO: Add Mana Ability
        }
    }
}

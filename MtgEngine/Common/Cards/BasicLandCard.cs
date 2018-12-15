using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract class BasicLandCard : LandCard
    {
        ManaColor _color;

        protected BasicLandCard(Player owner, ManaColor color, string name, string image, string cardId, CardType[] types, string[] subtypes) : 
            base(owner, name, image, cardId, types, subtypes, true, false)
        {
            _color = color;

            // TODO: Add Mana Ability
        }
    }
}

using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Black Knight", "LEA", "", "", FlavorText = "Battle doesn’t need a purpose; the battle is its own purpose. You don’t ask why a plague spreads or a field burns. Don’t ask why I fight.")]
    public class BlackKnight : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Human", "Knight" }, false, 2, 2, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{B}{B}");

            //TODO: StaticAbilities.Add(StaticAbility.ProtectionFromWhite);
            card.StaticAbilities.Add(StaticAbility.FirstStrike);

            return card;
        }
    }
}

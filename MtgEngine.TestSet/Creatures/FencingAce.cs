using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Fencing Ace", "TestSet", "", "", "Double strike (This creature deals both first-strike and regular combat damage.)", "His prowess gives the guildless hope that they can hold out against tyranny.")]
    public class FencingAce : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Human", "Soldier" }, false, 1, 1, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());
        
            card.Cost = ManaCost.Parse(card, "{1}{W}");
            card.AddStaticAbility(StaticAbility.DoubleStrike);

            return card;
        }
    }
}

using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Darksteel Myr", "TestSet", "", "", "Indestructible (Damage and effects that say \"destroy\" don't destroy this creature. If its toughness is 0 or less, it's still put into its owner's graveyard.)", "")]
    public class DarksteelMyr : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact, CardType.Creature }, new[] { "Myr" }, false, 0, 1, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{3}");
            card.AddStaticAbility(StaticAbility.Indestructible);

            return card;
        }
    }
}

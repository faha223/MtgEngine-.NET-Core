using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Boros Recruit", "TestSet", "", "", "({R/W} can be paid with either {R} or {W})\nFirststrike", "\"Look at this goblin, once wild, untamed, and full of idiocy. The Boros have given him the skill of a soldier and focused his heart with a single cause.\" -Razia")]
    public class BorosRecruit : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Goblin", "Soldier" }, false, 1, 1, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{R/W}");
            card.StaticAbilities.Add(StaticAbility.FirstStrike);

            return card;
        }
    }
}

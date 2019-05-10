using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Boros Recruit", "TestSet", "", "", "({R/W} can be paid with either {R} or {W})\nFirststrike", "\"Look at this goblin, once wild, untamed, and full of idiocy. The Boros have given him the skill of a soldier and focused his heart with a single cause.\" -Razia")]
    public class BorosRecruit : Card
    {
        public BorosRecruit(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Goblin", "Soldier"}, false, 1, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{R/W}");
            StaticAbilities.Add(StaticAbility.FirstStrike);
        }
    }
}

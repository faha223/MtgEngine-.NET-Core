using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Benalish Hero", "LEA", "", "", FlavorText = "Benalia has a complex caste system that changes with the lunar year. No matter what the season, the only caste that cannot be attained by either heredity or money is that of the hero.")]
    public class BenalishHero : Card
    {
        public BenalishHero(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Human", "Soldier" }, false, 1, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{W}");

            // TODO
            //StaticAbilities.Add(StaticAbility.Banding);
        }
    }
}

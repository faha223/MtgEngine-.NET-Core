using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    public class RavenousHarpy : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Harpy" }, false, 1, 2, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{2}{B}");
            card.StaticAbilities.Add(StaticAbility.Flying);
            card.AddAbility(new RavenousHarpyAbility(card));

            return card;
        }

        private class RavenousHarpyAbility : ActivatedAbility
        {
            public RavenousHarpyAbility(Card source) : base(source, new AggregateCost(source, ManaCost.Parse(source, "{1}"), new SacrificeAnotherCreatureCost(source)), "{1}, Sacrifice another creature: Put a +1/+1 counter on Ravenous Harpy")
            {
            }

            public override Ability Copy(Card newSource)
            {
                return new RavenousHarpyAbility(newSource);
            }

            public override void OnResolve(Game game)
            {
                Source.AddCounters(this, 1, Common.Enums.CounterType.Plus1Plus1);
            }
        }
    }
}

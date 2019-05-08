using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    public class RavenousHarpy : PermanentCard
    {
        public RavenousHarpy(Player owner) : base(owner, true, null, new[] { CardType.Creature }, new[] { "Harpy" }, false, 1, 2, false, false)
        {
            Cost = ManaCost.Parse(this, "{2}{B}");
            StaticAbilities.Add(StaticAbility.Flying);
            Abilities.Add(new RavenousHarpyAbility(this));
        }

        private class RavenousHarpyAbility : ActivatedAbility
        {
            public RavenousHarpyAbility(PermanentCard source) : base(source, new AggregateCost(source, ManaCost.Parse(source, "{1}"), new SacrificeAnotherCreatureCost(source)), "{1}, Sacrifice another creature: Put a +1/+1 counter on Ravenous Harpy")
            {
            }

            public override Ability Copy(PermanentCard newSource)
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

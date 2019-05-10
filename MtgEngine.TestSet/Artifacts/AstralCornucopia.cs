using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Counters;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;
using System.Linq;

namespace MtgEngine.TestSet
{
    [MtgCard("Astral Cornucopia", "TestSet", "", "", "Astral Cornucopia enters the battlefield with X charge counters on it.\n{T}: Choose a color. Add one mana of that color for each charge counter on Astral Cornucopia.")]
    public class AstralCornucopia : Card
    {
        private int _x;

        public AstralCornucopia(Player owner) : base(owner, new[] { CardType.Artifact }, null, false, false)
        {
            Cost = ManaCost.Parse(this, "{X}{X}{X}");

            // Subscribe to X event so that we know what the value for X was when this enters the battlefield
            var mc = Cost as ManaCost;
            mc.ValueforXChosen += (x => _x = x);

            Abilities.Add(new AstralCornucopiaManaAbility(this));
        }

        public override void OnResolve(Game game)
        {
            AddCounters(this, _x, CounterType.Charge);
        }

        private class AstralCornucopiaManaAbility : ManaAbility
        {
            public AstralCornucopiaManaAbility(Card source) : base(source, new TapCost(source), null, "{T}: Choose a color. Add one mana of that color for each charge counter on Astral Cornucopia.")
            {
            }

            public override void OnResolve(Game game)
            {
                var options = new[] { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green };
                var selection = Source.Controller.ChooseColor();
                Source.Controller.ManaPool.Add(new ManaAmount(Source.Counters.Count(c => c == CounterType.Charge), selection));
            }
        }
    }
}

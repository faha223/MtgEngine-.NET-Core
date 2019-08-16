using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;
using System.Linq;

namespace MtgEngine.TestSet
{
    [MtgCard("Astral Cornucopia", "TestSet", "", "", "Astral Cornucopia enters the battlefield with X charge counters on it.\n{T}: Choose a color. Add one mana of that color for each charge counter on Astral Cornucopia.")]
    public class AstralCornucopia : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact }, null, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{X}{X}{X}");

            var mc = card.Cost as ManaCost;
            mc.ValueforXChosen += (x => card.SetVar("X", x));

            card.AddAbility(new AstralCornucopiaManaAbility(card));

            card.OnResolve = (g, c) =>
            {
                c.AddCounters(c, c.GetVar<int>("X"), CounterType.Charge);
            };

            return card;
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

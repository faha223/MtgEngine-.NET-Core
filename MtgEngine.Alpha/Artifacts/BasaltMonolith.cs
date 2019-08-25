using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Mana;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    [MtgCard("Basalt Monolith", "LEA", "", "")]
    public class BasaltMonolith : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Artifact }, null, false, false);
            card._attrs = MtgCard;
        
            card.Cost = ManaCost.Parse(card, "{3}");

            // {T}: Add {C}{C}{C}
            card.AddAbility(new ManaAbility(card, new TapCost(card), new ManaAmount(3, ManaColor.Colorless), "{T}: Add {C}{C}{C}"));

            // {3}: Untap Basalt Monolith
            card.AddAbility(new BasaltMonolithUntapAbility(card));

            // Basalt monolith doesn't untap during your untap step
            card.UntapsDuringUntapStep = () => false;

            return card;
        }

        public class BasaltMonolithUntapAbility : ActivatedAbility
        {
            public BasaltMonolithUntapAbility(Card source) : base(source, null, "{3}: Untap Basalt Monolith")
            {
                Cost = ManaCost.Parse(this, "{3}");
            }

            public override Ability Copy(Card newSource)
            {
                return new BasaltMonolithUntapAbility(newSource);
            }

            public override void OnResolve(Game game)
            {
                Source.Untap();
            }
        }
    }
}

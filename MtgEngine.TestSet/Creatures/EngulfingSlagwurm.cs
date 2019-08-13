using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Engulfing Slagwurm", "TestSet", "", "", "Whenever Engulfing Slagwurm blocks or becomes blocked by a creature, destroy that creature. You gain life equal to that creature’s toughness.", "Its teeth exist only for decoration.")]
    public class EngulfingSlagwurm : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Creature }, new[] { "Wurm" }, false, 7, 7, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{5}{G}{G}");
            card.Abilities.Add(new EngulfingSlagwurmAbility(card));

            return card;
        }
    }

    public class EngulfingSlagwurmAbility : EventTriggeredAbility
    {
        Card otherPermanent;

        public EngulfingSlagwurmAbility(Card source) : base(source, "Whenever Engulfing Slagwurm blocks or becomes blocked by a creature, destroy that creature. You gain life equal to that creature’s toughness.")
        {
        }

        public override void BlockerDeclared(Game game, Card attacker, Card blocker)
        {
            if (attacker == Source)
                otherPermanent = blocker;
            else if (blocker == Source)
                otherPermanent = attacker;
            else
                return;
            game.AbilityTriggered(this);
        }

        public override void OnResolve(Game game)
        {
            int toughness = otherPermanent.Toughness;
            game.DestroyPermanent(otherPermanent);
            Source.Controller.GainLife(otherPermanent.Toughness);
        }

        public override Ability Copy(Card newSource)
        {
            return new EngulfingSlagwurmAbility(newSource);
        }
    }
}

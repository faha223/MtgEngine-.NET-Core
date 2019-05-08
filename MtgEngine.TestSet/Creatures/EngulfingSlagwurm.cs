using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Engulfing Slagwurm", "TestSet", "", "", "Whenever Engulfing Slagwurm blocks or becomes blocked by a creature, destroy that creature. You gain life equal to that creature’s toughness.", "Its teeth exist only for decoration.")]
    public class EngulfingSlagwurm : PermanentCard
    {
        public EngulfingSlagwurm(Player owner) : base(owner, true, null, new[] { CardType.Creature }, new[] { "Wurm" }, false, 7, 7, false, false)
        {
            Cost = ManaCost.Parse(this, "{5}{G}{G}");

            Abilities.Add(new EngulfingSlagwurmAbility(this));
        }
    }

    public class EngulfingSlagwurmAbility : EventTriggeredAbility
    {
        PermanentCard otherPermanent;

        public EngulfingSlagwurmAbility(PermanentCard source) : base(source, "Whenever Engulfing Slagwurm blocks or becomes blocked by a creature, destroy that creature. You gain life equal to that creature’s toughness.")
        {
        }

        public override void BlockerDeclared(Game game, PermanentCard attacker, PermanentCard blocker)
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

        public override Ability Copy(PermanentCard newSource)
        {
            return new EngulfingSlagwurmAbility(newSource);
        }
    }
}

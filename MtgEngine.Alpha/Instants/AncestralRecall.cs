using MtgEngine.Common;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Collections.Generic;

namespace MtgEngine.Alpha.Instants
{
    [MtgCard("Ancestral Recall", "LEA", "", "", Text = "Target player draws three cards.")]
    public class AncestralRecall : Card
    {
        Player _target;

        public AncestralRecall(Player owner) : base(owner, new[] { CardType.Instant }, null, false)
        {
            Cost = ManaCost.Parse(this, "{U}");
        }

        public override void OnCast(Game game)
        {
            _target = Controller.ChooseTarget(this, new List<ITarget>(game.Players())) as Player;
        }

        public override void OnResolve(Game game)
        {
            _target.Draw(3);
        }
    }
}

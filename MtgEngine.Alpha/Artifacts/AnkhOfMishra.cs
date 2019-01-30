using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Artifacts
{
    // TODO: Add a continuous effect that subscribes to Enters the Battlefield events
    [MtgCard("Ankh of Mishra", "LEA", "", "", Text = "Whenever a land enters the battlefield, Ankh of Mishra deals 2 damage to that land's controller.")]
    public class AnkhOfMishra : ArtifactCard
    {
        public AnkhOfMishra(Player owner) : base(owner, true, null, new[] { CardType.Artifact }, null, false, false)
        {
            Cost = ManaCost.Parse(this, "{2}");
        }
    }
}

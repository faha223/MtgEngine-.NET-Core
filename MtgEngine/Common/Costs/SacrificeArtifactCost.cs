namespace MtgEngine.Common.Costs
{
    public class SacrificeArtifactCost : SacrificeTargetCost
    {
        public SacrificeArtifactCost(IResolvable source) : base(source, card => card.IsAnArtifact, "Sacrifice an Artifact")
        {
        }
    }
}

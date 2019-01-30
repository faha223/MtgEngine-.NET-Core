namespace MtgEngine.Common.Costs
{
    public class SacrificeAnotherCreatureCost : SacrificeTargetCost
    {
        public SacrificeAnotherCreatureCost(IResolvable source) : base(source, card => card != source && card.IsACreature)
        {
        }
    }
}

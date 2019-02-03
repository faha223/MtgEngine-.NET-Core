namespace MtgEngine.Common.Costs
{
    public class SacrificeCreatureCost : SacrificeTargetCost
    {
        public SacrificeCreatureCost(IResolvable source) : base(source, card => card.IsACreature)
        {
        }

        public override string ToString()
        {
            return "Sacrifice a Creature";
        }
    }
}

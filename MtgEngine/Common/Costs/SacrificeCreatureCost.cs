namespace MtgEngine.Common.Costs
{
    public class SacrificeCreatureCost : SacrificeTargetCost
    {
        public SacrificeCreatureCost(IResolvable source) : base(source, card => card.IsACreature, "Sacrifice a Creature")
        {
        }

        public override string ToString()
        {
            return "Sacrifice a Creature";
        }
    }
}

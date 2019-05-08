namespace MtgEngine.Common.Costs
{
    public class NoCost : Cost
    {
        public NoCost(IResolvable source) : base(source)
        {
        }

        public override bool CanPay()
        {
            return true;
        }

        public override bool Pay()
        {
            return true;
        }

        public override Cost Copy(IResolvable newSource)
        {
            return new NoCost(newSource);
        }

        public override string ToString()
        {
            return "{0}";
        }
    }
}

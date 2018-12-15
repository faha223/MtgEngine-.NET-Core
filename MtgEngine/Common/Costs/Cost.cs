namespace MtgEngine.Common.Costs
{
    public abstract class Cost
    {
        protected readonly IResolvable _source;

        public abstract void Pay();

        public abstract bool CanPay();

        protected Cost(IResolvable source)
        {
            _source = source;
        }
    }
}

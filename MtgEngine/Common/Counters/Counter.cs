namespace MtgEngine.Common.Counters
{
    public abstract class Counter
    {
        public string Name
        {
            get
            {
                return CounterAttribute.GetAttribute(GetType())?.Name;
            }
        }
    }
}

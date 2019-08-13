namespace MtgEngine.Common.Damage
{
    public class DamageInfo
    {
        public IResolvable DamageSource { get; set; }
        public int DamageAmount { get; set; }

        public DamageInfo(IResolvable source, int amount) 
        {
            DamageSource = source;
            DamageAmount = amount;
        }
    }
}

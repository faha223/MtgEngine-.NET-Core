using MtgEngine.Common.Mana;
using System.Text;

namespace MtgEngine.Common.Costs
{
    public class ManaCost : Cost
    {
        private ManaAmount[] _manaAmounts;

        public ManaCost(IResolvable source, params ManaAmount[] manaAmounts) : base(source)
        {
            _manaAmounts = manaAmounts;
        }

        public ManaCost(IResolvable source, string manaCost) : base(source)
        {
            _manaAmounts = ManaParser.Parse(manaCost);
        }

        public override bool CanPay()
        {
            return false;
        }

        public override void Pay()
        {
            // TODO
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var amount in _manaAmounts)
                sb.Append(amount.ToString());
            return sb.ToString();
        }
    }
}

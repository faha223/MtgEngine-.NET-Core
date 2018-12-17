using MtgEngine.Common.Mana;
using System.Text;

namespace MtgEngine.Common.Costs
{
    // TODO: Make constructors private, add a static method that parses mana cost
    public class ManaCost : Cost
    {
        private ManaAmount[] _manaAmounts;

        private ManaCost(IResolvable source, params ManaAmount[] manaAmounts) : base(source)
        {
            _manaAmounts = manaAmounts;
        }

        public static Cost Parse(IResolvable source, string manaCost)
        {
            var manaAmounts = ManaParser.Parse(manaCost);
            if (manaAmounts == null)
                return new NoCost(source);
            return new ManaCost(source, manaAmounts);
        }

        public override bool CanPay()
        {
            // TODO
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

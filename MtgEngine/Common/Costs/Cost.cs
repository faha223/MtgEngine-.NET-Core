using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Costs
{
    public abstract class Cost
    {
        protected readonly IResolvable _source;

        protected Player _controller { get
            {
                if (_source != null)
                {
                    if (_source is Card)
                        return (_source as Card).Controller;
                    else if (_source is Ability)
                        return (_source as Ability).Source.Controller;
                }
                return null;
            }
        }

        public abstract bool Pay();

        public abstract bool CanPay();

        protected Cost(IResolvable source)
        {
            _source = source;
        }
    }
}

using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Modifiers
{
    public class CostModifier : Modifier
    {
        public Cost Value { get; set; }

        public CostModifier(IResolvable source, string property, Cost value) : base(source, property, ModifierMode.Override)
        {
            Value = value;
        }
    }
}

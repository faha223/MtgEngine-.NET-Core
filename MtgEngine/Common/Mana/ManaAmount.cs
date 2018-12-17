using MtgEngine.Common.Enums;
using System.Text;

namespace MtgEngine.Common.Mana
{
    public class ManaAmount
    {
        public int Amount { get; private set; }
        public ManaColor Color { get; private set; }
        public override string ToString()
        {
            if (Amount == 0)
                return "{0}";
            
            if(Color == ManaColor.Generic)
                return $"{{{Amount}}}";
            else
            {
                var attrib = ManaAttribute.GetManaAttribute(Color);
                var sb = new StringBuilder();
                for (int i = 0; i < Amount; i++)
                    sb.Append(attrib.AsString);

                return sb.ToString();
            }
        }
    }
}

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
            string colorStr = null;
            switch(Color)
            {
                case ManaColor.White:
                    colorStr = "{W}";
                    break;
                case ManaColor.Blue:
                    colorStr = "{U}";
                    break;
                case ManaColor.Black:
                    colorStr = "{B}";
                    break;
                case ManaColor.Red:
                    colorStr = "{R}";
                    break;
                case ManaColor.Green:
                    colorStr = "{G}";
                    break;
                case ManaColor.Colorless:
                    colorStr = "{C}";
                    break;
                default:
                    return $"{{{Amount}}}";
            }

            var sb = new StringBuilder();
            for(int i = 0; i < Amount; i++)
                sb.Append(colorStr);

            return sb.ToString();
        }
    }
}

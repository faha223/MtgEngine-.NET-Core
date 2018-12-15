using System;

namespace MtgEngine.Common.Cards
{
    public class MtgCardAttribute : Attribute
    {
        public string Name;
        public string SetName;

        public MtgCardAttribute(string Name, string SetName)
        {
            this.Name = Name;
            this.SetName = SetName;
        }
    }
}
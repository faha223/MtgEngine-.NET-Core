using System;

namespace MtgEngine.Common.Cards
{
    public class MtgCardAttribute : Attribute
    {
        public string Name;
        public string SetName;
        public string ImageUri;
        public string CardId;
        public string Text;
        public string FlavorText;

        public MtgCardAttribute(string Name, string SetName, string ImageUri, string CardId, string Text = null, string FlavorText = null)
        {
            this.Name = Name;
            this.SetName = SetName;
            this.ImageUri = ImageUri;
            this.CardId = CardId;
            this.Text = Text;
            this.FlavorText = FlavorText;
        }
    }
}
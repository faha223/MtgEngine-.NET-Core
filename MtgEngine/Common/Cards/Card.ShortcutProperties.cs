using System;
using System.Linq;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public partial class Card : ITarget, IResolvable
    {
        public string CardId => MtgCardAttribute.GetAttribute(GetType())?.CardId;

        public virtual string Name => MtgCardAttribute.GetAttribute(GetType())?.Name;

        public virtual string Set => MtgCardAttribute.GetAttribute(GetType())?.SetName;

        public virtual string ImageUri => MtgCardAttribute.GetAttribute(GetType())?.ImageUri;

        public virtual string Text => MtgCardAttribute.GetAttribute(GetType())?.Text;

        public virtual string FlavorText => MtgCardAttribute.GetAttribute(GetType())?.FlavorText;

        public bool IsACreature => Types.Contains(CardType.Creature);

        public bool IsAnArtifact => Types.Contains(CardType.Artifact);

        public bool IsALand => Types.Contains(CardType.Land);

        public bool IsAnEnchantment => Types.Contains(CardType.Enchantment);

        public bool IsAnInstant => Types.Contains(CardType.Instant);

        public bool IsASorcery => Types.Contains(CardType.Instant);

        public bool IsAPlaneswalker => Types.Contains(CardType.Planeswalker);

        public bool IsATribal => Types.Contains(CardType.Tribal);
    }
}

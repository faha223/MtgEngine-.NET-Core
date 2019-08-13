using System;
using System.Linq;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public partial class Card : ITarget, IResolvable
    {
        public MtgCardAttribute _attrs { get; set; }

        public string CardId => _attrs?.CardId;

        public string Name => _attrs?.Name;

        public string Set => _attrs?.SetName;

        public string ImageUri => _attrs?.ImageUri;

        public string Text => _attrs?.Text;

        public string FlavorText => _attrs?.FlavorText;

        public bool IsACreature => Types.Contains(CardType.Creature);

        public bool IsAnArtifact => Types.Contains(CardType.Artifact);

        public bool IsALand => Types.Contains(CardType.Land);

        public bool IsAnEnchantment => Types.Contains(CardType.Enchantment);

        public bool IsAnInstant => Types.Contains(CardType.Instant);

        public bool IsASorcery => Types.Contains(CardType.Instant);

        public bool IsAPlaneswalker => Types.Contains(CardType.Planeswalker);

        public bool IsATribal => Types.Contains(CardType.Tribal);

        public bool IsAPermanent => IsAnArtifact || IsACreature || IsAnEnchantment || IsALand || IsAPlaneswalker;

        public bool IsASpell => IsAnInstant || IsASorcery;
    }
}

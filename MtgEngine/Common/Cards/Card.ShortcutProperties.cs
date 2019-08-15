﻿using System;
using System.Collections.Generic;
using System.Linq;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Modifiers;

namespace MtgEngine.Common.Cards
{
    public partial class Card : ITarget, IResolvable
    {
        public MtgCardAttribute _attrs { get; set; }

        public List<Modifier> Modifiers { get; } = new List<Modifier>();

        public string CardId => _attrs?.CardId;

        public string PrintedName => _attrs?.Name;
        public string Name
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(Name)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(Name));
                    return (modifier as StringModifier).Value;
                }
                return PrintedName;
            }
        }

        public string PrintedSet => _attrs?.SetName;
        public string Set
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(Set)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(Set));
                    return (modifier as StringModifier).Value;
                }
                return PrintedSet;
            }
        }

        public string PrintedImageUri => _attrs?.ImageUri;
        public string ImageUri
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(ImageUri)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(ImageUri));
                    return (modifier as StringModifier).Value;
                }
                return PrintedImageUri;
            }
        }

        public string PrintedText => _attrs?.Text;
        public string Text
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(Text)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(Text));
                    return (modifier as StringModifier).Value;
                }
                return PrintedText;
            }
        }

        public string PrintedFlavorText => _attrs?.FlavorText;
        public string FlavorText
        {
            get
            {
                if (Modifiers.Any(c => c.Property == nameof(FlavorText)))
                {
                    var modifier = Modifiers.Last(c => c.Property == nameof(FlavorText));
                    return (modifier as StringModifier).Value;
                }
                return PrintedFlavorText;
            }
        }

        public bool IsACreature => Types.Contains(CardType.Creature);

        public bool IsAnArtifact => Types.Contains(CardType.Artifact);

        public bool IsALand => Types.Contains(CardType.Land);

        public bool IsAnEnchantment => Types.Contains(CardType.Enchantment);

        public bool IsAnInstant => Types.Contains(CardType.Instant);

        public bool IsASorcery => Types.Contains(CardType.Instant);

        public bool IsAPlaneswalker => Types.Contains(CardType.Planeswalker);

        public bool IsATribal => Types.Contains(CardType.Tribal);

        public bool IsAToken => Types.Contains(CardType.Token);

        public bool IsAPermanent => IsAnArtifact || IsACreature || IsAnEnchantment || IsALand || IsAPlaneswalker;

        public bool IsASpell => IsAnInstant || IsASorcery;
    }
}

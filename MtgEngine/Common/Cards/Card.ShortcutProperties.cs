﻿using System;
using System.Linq;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract partial class Card : ITarget, IResolvable
    {
        public string CardId => MtgCardAttribute.GetAttribute(GetType())?.CardId;

        public string Name => MtgCardAttribute.GetAttribute(GetType())?.Name;

        public string Set => MtgCardAttribute.GetAttribute(GetType())?.SetName;

        public string ImageUri => MtgCardAttribute.GetAttribute(GetType())?.ImageUri;

        public string Text => MtgCardAttribute.GetAttribute(GetType())?.Text;

        public string FlavorText => MtgCardAttribute.GetAttribute(GetType())?.FlavorText;

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
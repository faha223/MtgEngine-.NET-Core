using MtgEngine.Common.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common
{
    public class Zone : List<Card>
    {
        public IEnumerable<CreatureCard> Creatures { get => this.Where(c => c is CreatureCard).Select(c => c as CreatureCard); }

        public IEnumerable<PermanentCard> Artifacts { get => this.Where(c => c is PermanentCard && c.Types.Contains(CardType.Artifact)).Select(c => c as PermanentCard); }

        public IEnumerable<PermanentCard> Enchantments { get => this.Where(c => c is PermanentCard && c.Types.Contains(CardType.Enchantment)).Select(c => c as PermanentCard); }

        public IEnumerable<SpellCard> Instants { get => this.Where(c => c is SpellCard && c.Types.Contains(CardType.Instant)).Select(c => c as SpellCard); }

        public IEnumerable<SpellCard> Sorceries { get => this.Where(c => c is SpellCard && c.Types.Contains(CardType.Sorcery)).Select(c => c as SpellCard); }

        public IEnumerable<LandCard> Lands { get => this.Where(c => c is LandCard).Select(c => c as LandCard); }

        public IEnumerable<PermanentCard> NonLandPermanents { get => this.Where(c => c is PermanentCard && !(c is LandCard)).Select(c => c as PermanentCard); }
    }
}

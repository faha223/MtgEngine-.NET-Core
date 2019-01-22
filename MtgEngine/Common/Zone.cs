using MtgEngine.Common.Cards;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Common
{
    public class Zone : List<Card>
    {
        public IEnumerable<PermanentCard> Creatures { get => this.Where(c => c.IsACreature && c is PermanentCard).Select(c => c as PermanentCard); }

        public IEnumerable<PlaneswalkerCard> Planeswalkers { get => this.Where(c => c.IsAPlaneswalker).Select(c => c as PlaneswalkerCard); }

        public IEnumerable<PermanentCard> Artifacts { get => this.Where(c => c is PermanentCard && c.IsAnArtifact).Select(c => c as PermanentCard); }

        public IEnumerable<PermanentCard> Enchantments { get => this.Where(c => c is PermanentCard && c.IsAnEnchantment).Select(c => c as PermanentCard); }

        public IEnumerable<SpellCard> Instants { get => this.Where(c => c is SpellCard && c.IsAnInstant).Select(c => c as SpellCard); }

        public IEnumerable<SpellCard> Sorceries { get => this.Where(c => c is SpellCard && c.IsASorcery).Select(c => c as SpellCard); }

        public IEnumerable<PermanentCard> Lands { get => this.Where(c => c is PermanentCard && c.IsALand).Select(c => c as PermanentCard); }

        public IEnumerable<PermanentCard> NonLandPermanents { get => this.Where(c => c is PermanentCard && !c.IsALand).Select(c => c as PermanentCard); }
    }
}

using MtgEngine.Common.Cards;
using System.Collections.Generic;
using System.Linq;

namespace MtgEngine.Common
{
    public class Zone : List<Card>
    {
        public IEnumerable<Card> Creatures { get => this.Where(c => c.IsACreature); }

        public IEnumerable<Card> Planeswalkers { get => this.Where(c => c.IsAPlaneswalker); }

        public IEnumerable<Card> Artifacts { get => this.Where(c => c.IsAnArtifact); }

        public IEnumerable<Card> Enchantments { get => this.Where(c => c.IsAnEnchantment); }

        public IEnumerable<Card> Instants { get => this.Where(c => c.IsAnInstant); }

        public IEnumerable<Card> Sorceries { get => this.Where(c => c.IsASorcery); }

        public IEnumerable<Card> Lands { get => this.Where(c => c.IsALand); }

        public IEnumerable<Card> NonLandPermanents { get => this.Where(c => !c.IsALand && !c.IsASorcery && !c.IsAnInstant); }
    }
}

using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Sorceries
{
    [MtgCard("Armageddon", "LEA", "", "", Text = "Destroy all lands.")]
    public class Armageddon : CardSource
    {
        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Sorcery }, null, false);
            card._attrs = CardAttrs;
            card.Cost = ManaCost.Parse(card, "{3}{W}");

            card.OnResolve = (g, c) =>
            {
                g.DestroyLands(_c => true);
            };

            return card;
        }
    }
}

using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Cards
{
    public abstract class SpellCard : Card
    {
        public SpellCard(Player owner, string name, string image, string cardId, Cost cost, CardType[] types, string[] subtypes, bool isLegendary) :
            base(owner, name, image, cardId, true, cost, types, subtypes, false, isLegendary)
        {
        }

        public override void AfterResolve(Game game)
        {
            game.ChangeZone(this, Zone.Graveyard);
        }
    }
}

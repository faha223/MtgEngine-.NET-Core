using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Creatures
{
    [MtgCard("Savannah Lions", "LEA", "", "", FlavorText = "The traditional kings of the jungle command a healthy respect in other climates as well. Relying mainly on their stealth and speed, Savannah Lions can take a victim by surprise, even in the endless flat plains of their homeland.")]
    public class SavannahLions : Card
    {
        public SavannahLions(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Cat" }, false, 2, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{W}");
        }
    }
}

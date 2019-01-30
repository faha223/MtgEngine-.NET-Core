using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Sorceries
{
    [MtgCard("Armageddon", "LEA", "", "", Text = "Destroy all lands.")]
    public class Armageddon : SpellCard
    {
        public Armageddon(Player owner) : base(owner, null, new[] { CardType.Sorcery }, null, false)
        {
            Cost = ManaCost.Parse(this, "{3}{W}");
        }

        public override void OnResolve(Game game)
        {
            game.DestroyLands(c => true);
        }
    }
}

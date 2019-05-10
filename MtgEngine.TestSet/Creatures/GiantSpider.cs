using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Giant Spider", "TestSet", "", "", "Reach (This creature can block creatures with flying.)", "\"After everything I’ve survived, it’s hard to be frightened by anything anymore.\"\n-Vivien Reid")]
    public class GiantSpider : Card
    {
        public GiantSpider(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Spider" }, false, 2, 4, false, false)
        {
            Cost = ManaCost.Parse(this, "{3}{G}");
            StaticAbilities.Add(StaticAbility.Reach);
        }
    }
}

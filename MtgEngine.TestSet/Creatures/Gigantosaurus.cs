using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Gigantosaurus", "TestSet", "", "", null, "\"Each tooth is the length of a horse, and new ones grow in every sixteen days.\nLet\'s get a closer look!\"\n-Vivien Reid")]
    public class Gigantosaurus : Card
    {
        public Gigantosaurus(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Dinosaur" }, false, 10, 10, false, false)
        {
            Cost = ManaCost.Parse(this, "{G}{G}{G}{G}{G}");
        }
    }
}

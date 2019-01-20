using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet
{
    [MtgCard("Grizzly Bears", "TestSet", "", "", null, "\"We cannot forget that among all of Dominaria's wonders, a system of life exists, with prey and predators that will never fight wars nor vie for ancient power.\"\n-Jorael, empress of beasts")]
    public class GrizzlyBears : PermanentCard
    {
        public GrizzlyBears(Player owner) : base(owner, true, null, new CardType[] { CardType.Creature }, new string[] { "Bear" }, false, 2, 2, false, false)
        {
            Cost = ManaCost.Parse(this, "{1}{G}");
        }
    }
}

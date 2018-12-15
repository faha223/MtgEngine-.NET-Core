using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;

namespace MtgEngine.TestSet
{ 
    [MtgCard("Forest", "TestSet")]
    public class Forest : BasicLandCard
    {
        public Forest(Player owner) : base(owner, ManaColor.Green, "Forest", "", "", new[] { CardType.Land }, new[] { "Forest" })
        {

        }

        public override void AfterResolve(Game game)
        {
            game.ChangeZone(this, Zone.Battlefield);
        }
    }
}

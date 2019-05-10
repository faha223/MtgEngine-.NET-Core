using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.TestSet.Creatures
{
    [MtgCard("Brushstrider", "TestSet", "", "", "Vigilance", "Magistrate Ludy agreed to designate land for the brushstriders only after several broken windows and dozens of missing blini-cakes.")]
    public class Brushstrider : Card
    {
        public Brushstrider(Player owner) : base(owner, new[] { CardType.Creature }, new[] { "Beast" }, false, 3, 1, false, false)
        {
            Cost = ManaCost.Parse(this, "{1}{G}");
            StaticAbilities.Add(StaticAbility.Vigilance);
        }
    }
}

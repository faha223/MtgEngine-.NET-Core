using MtgEngine.Common;
using MtgEngine.Common.Abilities;
using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;
using System.Linq;

namespace MtgEngine.Alpha.Enchantments
{ 
    [MtgCard("Black Ward", "LEA", "", "", Text= "Enchant creature\n\nEnchanted creature has protection from black.This effect doesn’t remove Black Ward.")]
    public class BlackWard : Card, ITargeting
    {
        public Card enchantedCreature;

        public BlackWard(Player owner) : base(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false, false, false)
        {
            Cost = ManaCost.Parse(this, "{W}");
        }

        public void SelectTargets(Game game)
        {
            var possibleTargets = game.Battlefield.Creatures.AsEnumerable<ITarget>().ToList();
            enchantedCreature = (Card)Controller.ChooseTarget(this, possibleTargets);
        }

        public override void OnResolve(Game game)
        {
            // TODO: Give Enchanted Creature Protecton from Black
        }
    }
}

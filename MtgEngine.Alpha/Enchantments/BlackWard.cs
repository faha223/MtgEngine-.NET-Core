using MtgEngine.Common.Cards;
using MtgEngine.Common.Costs;
using MtgEngine.Common.Enums;
using MtgEngine.Common.Players;

namespace MtgEngine.Alpha.Enchantments
{ 
    [MtgCard("Black Ward", "LEA", "", "", Text= "Enchant creature\n\nEnchanted creature has protection from black.This effect doesn’t remove Black Ward.")]
    public class BlackWard : CardSource
    {
        public Card enchantedCreature;

        public override Card GetCard(Player owner)
        {
            var card = new Card(owner, new[] { CardType.Enchantment }, new[] { "Aura" }, false, false, false);
            card._attrs = MtgCardAttribute.GetAttribute(GetType());

            card.Cost = ManaCost.Parse(card, "{W}");

            card.OnCast = game =>
            {
                var target = card.Controller.SelectTarget("Enchant Creature", (c) => c.IsACreature);
                card.SetVar("Target", target);
            };

            card.OnResolve = Game =>
            {
                var target = card.GetVar<Card>("Target");
                // TODO: Attach Aura to Target
            };

            return card;
        }
    }
}

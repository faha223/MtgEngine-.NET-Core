using MtgEngine.Common.Cards;
using MtgEngine.Common.Enums;

namespace MtgEngine.Common.Abilities
{
    public abstract class EnchantPermanentAbility : Ability
    {
        protected EnchantPermanentAbility(Card source, string text) : base(source, text)
        {
        }

        public override void OnResolve(Game game)
        {
            //var target = Source.Controller.ChooseTarget(this, new List<ITarget>(game.Battlefield.Creatures.Where(c => c.CanBeTargetedBy(this)))) as Card;
            var target = Source.GetVar<Card>("Target");

            // Destroy this enchantment if the target is null or is no longer a legal target
            if (target == null || !target.CanBeTargetedBy(this) || !game.Battlefield.Contains(target))
                game.MoveFromBattlefieldToGraveyard(Source);
            else
                Enchant(target);
        }

        public abstract void Enchant(Card target);

        public abstract void Disenchant(Card target);
    }
}

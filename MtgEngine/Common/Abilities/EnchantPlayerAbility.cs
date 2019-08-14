using MtgEngine.Common.Cards;
using MtgEngine.Common.Players;

namespace MtgEngine.Common.Abilities
{
    public abstract class EnchantPlayerAbility : Ability
    {
        protected EnchantPlayerAbility(Card source, string text) : base(source, text)
        {
        }

        public override void OnResolve(Game game)
        {
            var target = Source.GetVar<Player>("Target");

            if (target == null)
                game.MoveFromBattlefieldToGraveyard(Source);
            else
                Enchant(target);
        }

        public abstract void Enchant(Player player);

        public abstract void Disenchant(Player player);
    }
}

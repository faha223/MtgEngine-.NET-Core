using MtgEngine.Common.Abilities;

namespace MtgEngine.Common.Players.Actions
{
    public class ActivateAbilityAction : ActionBase
    {
        public ActivatedAbility Ability { get; private set; }

        public ActivateAbilityAction(ActivatedAbility ability) : base(ActionType.ActivateAbility)
        {
            Ability = ability;
        }
    }
}

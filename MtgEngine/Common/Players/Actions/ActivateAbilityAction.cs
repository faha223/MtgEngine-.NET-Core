namespace MtgEngine.Common.Players.Actions
{
    public class ActivateAbilityAction : ActionBase
    {
        public Ability Ability { get; private set; }

        public ActivateAbilityAction(Ability ability) : base(ActionType.ActivateAbility)
        {
            Ability = ability;
        }
    }
}

namespace MtgEngine.Common.Players.Actions
{
    public abstract class ActionBase
    {
        public ActionType ActionType { get; private set; }

        protected ActionBase(ActionType actionType)
        {
            ActionType = actionType;
        }
    }
}

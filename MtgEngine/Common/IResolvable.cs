namespace MtgEngine.Common
{
    /// <summary>
    /// IResolvables are anything that can be put onto and resolved from the Stack
    /// </summary>
    public interface IResolvable
    {
        void OnResolve(Game game);
    }
}

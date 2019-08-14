using MtgEngine.Common.Players;

namespace MtgEngine.Common
{
    /// <summary>
    /// IResolvables are anything that can be put onto and resolved from the Stack
    /// </summary>
    public interface IResolvable
    {
        Player Controller { get; }
    }
}

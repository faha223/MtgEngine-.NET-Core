namespace MtgEngine.Common.Enums
{
    public enum ModifierMode
    {
        Add,        // Used for abilities like "gains Trample"
        Override,   // Used for abilities like "loses all abilities"
        Remove      // Used for abilities like "loses flying and can't gain flying"
    }
}

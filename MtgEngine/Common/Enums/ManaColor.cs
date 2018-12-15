namespace MtgEngine.Common.Enums
{
    public enum ManaColor
    {
        White,          // Must be paid with White mana sources
        Blue,           // Must be paid with Blue mana sources
        Black,          // Must be paid with Black mana sources
        Red,            // Must be paid with Red mana sources
        Green,          // Must be paid with Green mana sources
        Colorless,      // Must be paid with Colorless mana sources, cannot be generated from "Any Color" mana sources
        Generic         // May be paid with any mana source, cannot be explicitly generated
    }
}

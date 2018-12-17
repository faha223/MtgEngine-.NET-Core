namespace MtgEngine.Common.Mana
{
    public static class ManaParser
    {
        public static ManaAmount[] Parse(string manaCost)
        {
            // If the mana cost is 0, then there isn't really a mana cost to be paid
            if (manaCost == "{0}")
                return null;

            // TODO: Add Logic to parse Mana Costs, 
            // Suggested approach: maybe use a regex to extract matches from the string, and then
            // convert them to ManaAmounts in a list, and then return the list as an array
            return null;
        }
    }
}

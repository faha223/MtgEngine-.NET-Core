using System.Collections.Generic;

namespace MtgEngine.Common.Utilities
{
    public static class ListExtensions
    {
        public static List<T> StartAt<T>(this List<T> self, T startingElement)
        {
            int startingIndex = self.IndexOf(startingElement);
            if (startingIndex == 0)
                return self;

            var ordered = new List<T>(self.Count);
            for (int i = 0; i < self.Count; i++)
            {
                var index = (i + startingIndex) % self.Count;
                ordered.Add(self[index]);
            }

            return ordered;
        }
    }
}

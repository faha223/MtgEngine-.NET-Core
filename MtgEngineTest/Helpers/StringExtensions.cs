using System;
using System.Collections.Generic;

namespace MtgEngineTest.Helpers
{
    internal static class StringExtensions
    {
        public static string Wrap(this string str, int lineLength)
        {
            if (str == null)
                return null;

            str = str.Trim();
            List<string> lines = new List<string>();
            while (str.Length > lineLength)
            {
                int offset = lineLength;
                while (!string.IsNullOrWhiteSpace(str.Substring(offset, 1)))
                    offset--;

                lines.Add(str.Substring(0, offset));
                str = str.Substring(offset).Trim();
            }
            lines.Add(str);

            return string.Join(Environment.NewLine, lines);
        }
    }
}

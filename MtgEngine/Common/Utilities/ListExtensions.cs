﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        public static List<T> OrderByIndexList<T>(this List<T> self, IEnumerable<int> indexList)
        {
            var temp = new List<T>(indexList.Count());
            foreach(var i in indexList)
            {
                temp.Add(self[i]);
            }
            return temp;
        }

        public static T Random<T>(this List<T> self)
        {
            var rand = new Random();

            return self[Math.Abs(rand.Next()) % self.Count];
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var x in self)
                action(x);
        }
    }
}

using System;
using System.Linq;

namespace MtgEngine.Common.Counters
{
    class CounterAttribute : Attribute
    {
        public string Name { get; }
        public CounterAttribute(string name)
        {
            Name = name;
        }

        public static CounterAttribute GetAttribute(Type counterType)
        {
            var attributes = counterType.GetCustomAttributes(typeof(CounterAttribute), true);
            return attributes.FirstOrDefault(c => c is CounterAttribute) as CounterAttribute;
        }
    }
}

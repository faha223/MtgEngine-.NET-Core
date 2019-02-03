using MtgEngine.Common.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace MtgEngine.Common.Counters
{
    class CounterAttribute : Attribute
    {
        public string Name { get; }
        public CounterAttribute(string name)
        {
            Name = name;
        }

        public static CounterAttribute GetCounterAttribute(CounterType counter)
        {
            MemberInfo memberInfo = typeof(CounterType).GetMember(counter.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                return memberInfo.GetCustomAttribute(typeof(CounterAttribute), false) as CounterAttribute;
            }
            return null;
        }
    }
}

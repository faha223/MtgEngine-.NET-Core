using MtgEngine.Common.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace MtgEngine.Common.Mana
{
    class ManaAttribute : Attribute
    {
        public string AsString { get; }

        public int CMC { get; }

        public ManaAttribute(string asString, int cmc = 1)
        {
            AsString = asString;
            CMC = cmc;
        }

        public static ManaAttribute GetManaAttribute(ManaColor color)
        {
            MemberInfo memberInfo = typeof(ManaColor).GetMember(color.ToString()).FirstOrDefault();
            if(memberInfo != null)
            {
                return memberInfo.GetCustomAttribute(typeof(ManaAttribute), false) as ManaAttribute;
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public static class EnumUtil<T>
    {
        public static T Parse(string value)
        {
            return Parse(value, true);
        }

        public static T Parse(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static T Parse(int value)
        {
            return Parse(value.ToString());
        }
    }
}

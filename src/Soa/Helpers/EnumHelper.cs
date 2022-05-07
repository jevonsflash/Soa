using System;
using System.ComponentModel;
using System.Reflection;

namespace Soa.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            if (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute))
            {
                return name;
            }
            return attribute?.Description;
        }

        public static Enum GetEnum(this string description, Type type)
        {
            foreach (Enum item in Enum.GetValues(type))
            {
                if (description.Equals(item.GetDescription()) == true)
                    return item;
            }
            throw new Exception("没有找到对应的Enum");
        }
    }
}

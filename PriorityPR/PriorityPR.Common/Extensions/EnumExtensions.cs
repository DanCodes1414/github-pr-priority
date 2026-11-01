using System.ComponentModel;

namespace PriorityPR.Common.Extensions;

public static class EnumExtensions
{
    public static T GetEnumFromDescription<T>(this string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                {
                    return (T) field.GetValue(null);
                }
            }
            else
            {
                if (field.Name == description)
                {
                    return (T) field.GetValue(null);
                }
            }
        }

        return default;
    }

    public static string GetEnumDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());

        if (fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
}
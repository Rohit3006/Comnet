using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Comnet.Common.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static IEnumerable<EnumNameValueInInt> GetNameAndValuesInInt<T>()
        {
            return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(v => new EnumNameValueInInt { Name = v!.ToString(), Value = Convert.ToInt32(v) })
            .ToList();
        }

        public static IEnumerable<EnumNameValueInString> GetNameAndValuesInString<T>()
        {
            return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(v => new EnumNameValueInString { Name = v!.ToString(), Value = GetEnumStringValue((v as Enum)!) })
            .ToList();
        }

        public static string GetEnumStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            MemberInfo memberInfo = type.GetField(value.ToString())!;

            // Get the stringvalue attributes
            return memberInfo!.GetCustomAttributes(true).Length > 0 ? ((StringValueAttribute)memberInfo.GetCustomAttributes(true)[0]).StringValue : null!;
        }

        public static T ParseEnum<T>(string value)
        {
            value = value.Replace(" ", "");
            return (T)Enum.Parse(typeof(T), value, true);

        }

        public static string ParseEnumByValueReturnString<T>(int value)
        {
            return Enum.GetName(typeof(T), value)!;
        }

        public static T ParseEnumByValueReturnEnum<T>(int value)
        {
            var enumStringValue = Enum.GetName(typeof(T), value);
            var enumValue = ParseEnum<T>(enumStringValue!);
            return enumValue;
        }

        public static T GetValueFromString<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(StringValueAttribute)) as StringValueAttribute;
                if (attribute != null)
                {
                    if (attribute.StringValue.ToLower() == description.ToLower())
                        return (T)field.GetValue(null)!;
                }
                else
                {
                    if (field.Name.ToLower() == description.ToLower())
                        return (T)field.GetValue(null)!;
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        // Get Enum Attribute Description 
        public static string GetDescription(this Enum enumValue)
        {
            var Description =
                enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()!
                .GetCustomAttribute<DisplayAttribute>()?
                .GetDescription();

            if (String.IsNullOrEmpty(Description))
            {
                Description = enumValue.ToString();
            }
            return Description;
        }

        // Get Enum Attribute DisplayName
        public static string GetDisplayName(this Enum enumValue)
        {
            string? displayName;
            displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()!
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();
            if (String.IsNullOrEmpty(displayName))
            {
                displayName = enumValue.ToString();
            }
            return displayName;
        }
    }

    public class EnumNameValueInInt
    {
        public string? Name { get; set; }
        public int? Value { get; set; }
    }

    public class EnumNameValueInString
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
    }

}

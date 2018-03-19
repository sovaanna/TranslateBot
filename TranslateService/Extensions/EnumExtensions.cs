using System;
using System.Reflection;
using TranslateService.Attributes;

namespace TranslateService.Extensions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttribute<StringValueAttribute>();

            return attributes?.StringValue;
        }
    }
}

using System;
using ToDo.DomainModel.Attributes;

namespace ToDo.DomainModel.Extensions
{
    /// <summary>
    /// Extension for <see cref="Enum"/> for getting string value of enum item.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value">Enum item.</param>
        /// <returns>String representation of enum item.</returns>
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attributes?.Length > 0 ? attributes[0].StringValue : string.Empty;
        }
    }
}

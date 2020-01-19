using System;
using mat.coding.challenge.Attribute;

namespace mat.coding.challenge.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTopicName(this Type value)
        {
            // Get instance of the attribute.
            var topic =
                (TopicAttribute)System.Attribute.GetCustomAttribute(value, typeof(TopicAttribute));

            return topic != null ? topic.StringValue : string.Empty;
        }
    }
}

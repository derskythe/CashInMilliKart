using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Containers
{
    public class EnumEx
    {
        public static string GetStringFromArray(IEnumerable<Object> list)
        {
            var fields = new StringBuilder();
            if (list != null)
            {
                foreach (var o in list)
                {
                    if (o != null)
                    {
                        fields.Append(o).Append("\n");
                    }
                }
            }

            return fields.ToString();
        }

        public static string GetStringFromArray(IEnumerable<int> list)
        {
            var fields = new StringBuilder();
            if (list != null)
            {
                foreach (var o in list)
                {
                    fields.Append(o).Append("\n");
                }
            }

            return fields.ToString();
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }

        public static T[] ConvertToArray<T>(IEnumerable<T> values)
        {
            if (values == null)
            {
                return null;
            }

            var result = new List<T>();

            foreach (var value in values)
            {
                result.Add(value);
            }

            return result.ToArray();
        }

        public static String ToStringArray<T>(IEnumerable<T> array)
        {
            var result = new StringBuilder();
            Type typeParameterType = typeof(T);
            result.Append(String.Format("{0}: \n", typeParameterType.Name));

            foreach (var item in array)
            {
                result.Append(item).Append("\n");
            }

            return result.ToString();
        }
    }
}

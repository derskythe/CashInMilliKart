using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using NLog;

namespace Containers
{
    public class EnumEx
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable InconsistentNaming
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // ReSharper restore InconsistentNaming
        // ReSharper restore FieldCanBeMadeReadOnly.Local

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
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }

                FieldInfo field = value.GetType().GetField(value.ToString());

                if (field == null)
                {
                    return string.Empty;
                }
                var attribute = Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute))
                                as DescriptionAttribute;

                return attribute == null ? value.ToString() : attribute.Description;
            }
            catch (Exception exp)
            {
                Log.ErrorException(exp.Message, exp);
            }

            return String.Empty;
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

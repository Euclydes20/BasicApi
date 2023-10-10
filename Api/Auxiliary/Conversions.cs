using System.ComponentModel;

namespace Api.Auxiliary
{
    public static class Conversions
    {
        public static T To<T>(this object value)
        {
            Type conversionType = typeof(T);
            return (T)To(value, conversionType);
        }

        public static object To(this object value, Type conversionType)
        {
            if (conversionType == null)
                throw new ArgumentNullException("conversionType");

            // CAUSA ERRO QUANDO O TIPO NÃO É NULLABLE
            /*if (string.IsNullOrEmpty(value.ToString()))
                return null;*/

            if (string.IsNullOrEmpty(value.ToString()))
            {
                if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    return null;

                if (Extensions.IsNumericType(conversionType))
                    value = Activator.CreateInstance(conversionType);
            }

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                else if (value == DBNull.Value)
                    return null;
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            else if (conversionType == typeof(Guid))
            {
                return new Guid(value.ToString());
            }
            else if (conversionType == typeof(DateTime))
            {
                var formats = new[]
                {
                    "dd/MM/yyyy HH:mm:ss",
                    "dd/MM/yyyy hh:mm:ss",
                    "dd/MM/yyyy HH:mm:",
                    "dd/MM/yyyy h:mm",
                    "dd/MM/yyyy"
                };
                var converted = DateTime.TryParseExact(value.ToString(), formats, System.Globalization.CultureInfo.CreateSpecificCulture("pt-BR"), System.Globalization.DateTimeStyles.None, out DateTime dt);
                if (converted)
                    return dt;
                else
                    throw new InvalidOperationException("Não foi possível converter texto para DateTime.");
            }
            else if (conversionType == typeof(TimeSpan) && value.GetType() == typeof(string))
            {
                return TimeSpan.Parse(value.ToString());
            }
            else if (conversionType == typeof(Int64) && value.GetType() == typeof(int))
            {
                throw new InvalidOperationException("Não é possível converter Int64(long) para Int32(int).");
            }

            if ((value is string || value == null || value is DBNull) &&
                (conversionType == typeof(short) ||
                conversionType == typeof(int) ||
                conversionType == typeof(long) ||
                conversionType == typeof(double) ||
                conversionType == typeof(decimal) ||
                conversionType == typeof(float)))
            {
                decimal number;
                if (!decimal.TryParse(value as string, out number))
                    value = "0";
            }
            else if (value is string && conversionType == typeof(bool))
            {
                if (value.ToString().ToLower() == "true")
                    value = true;
                else if (value.ToString().ToLower() == "false")
                    value = false;
                else
                    throw new InvalidOperationException("Não foi possível converter texto para booleano.");
            }

            return Convert.ChangeType(value, conversionType);
        }

        static object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        public static Dictionary<string, int> EnumToDicionary<T>()
        {
            if (typeof(T).BaseType != typeof(Enum))
                return new Dictionary<string, int>();

            return Enum.GetValues(typeof(T)).Cast<int>().ToDictionary(currentEnum => Enum.GetName(typeof(T), currentEnum));
        }
    }
}

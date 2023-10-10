namespace Api.Auxiliary
{
    public class Extensions
    {
        public static bool IsNumericType(Type type, bool onlyInteger = false)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type underlyingType = Nullable.GetUnderlyingType(type);
                return IsNumericType(underlyingType);
            }

            return type == typeof(int)
                    || (type == typeof(float) && !onlyInteger)
                    || (type == typeof(double) && !onlyInteger)
                    || (type == typeof(decimal) && !onlyInteger)
                    || type == typeof(long)
                    || type == typeof(short)
                    || type == typeof(byte)
                    || type == typeof(uint)
                    || type == typeof(ulong)
                    || type == typeof(ushort)
                    || type == typeof(sbyte);
        }
    }
}

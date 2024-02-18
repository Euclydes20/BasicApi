using Api.Models;
using Api.Models.Exceptions;

namespace Api.Auxiliary
{
    public class Extensions
    {
        public static ResponseInfo<T> ResolveResponseException<T>(Exception ex, ResponseInfo<T> responseInfo)
        {
            if (ex is null)
                new ResponseException().ResolveResponseInfo(responseInfo);

            if (ex is ResponseException exRI)
                return exRI.ResolveResponseInfo(responseInfo);

            return new ResponseException(ex).ResolveResponseInfo(responseInfo);
        }

        public static ResponseInfo ResolveResponseException(Exception ex, ResponseInfo responseInfo)
        {
            if (ex is null)
                new ResponseException().ResolveResponseInfo(responseInfo);

            if (ex is ResponseException exRI)
                return exRI.ResolveResponseInfo(responseInfo);

            return new ResponseException(ex).ResolveResponseInfo(responseInfo);
        }

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

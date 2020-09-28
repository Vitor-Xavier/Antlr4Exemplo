using System;

namespace Antlr4Exemplo.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                case TypeCode.String:
                    return decimal.TryParse(o.ToString(), out decimal _);
                default:
                    return false;
            }
        }
    }
}

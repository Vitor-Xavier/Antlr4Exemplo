using Antlr4Exemplo.Extensions;
using System;
using System.Globalization;

namespace Antlr4Exemplo.Implementation
{
    public readonly struct ExemploValue
    {
        public readonly object Value { get; }

        public ExemploValue(object value) => Value = value;

        public static ExemploValue operator +(ExemploValue left, ExemploValue right) =>
            new ExemploValue((decimal)left + (decimal)right);

        public static ExemploValue operator -(ExemploValue left, ExemploValue right) =>
            new ExemploValue((decimal)left - (decimal)right);

        public static ExemploValue operator *(ExemploValue left, ExemploValue right) =>
            new ExemploValue((decimal)left * (decimal)right);

        public static ExemploValue operator /(ExemploValue left, ExemploValue right) => 
            new ExemploValue((decimal)left / (decimal)right);

        public static ExemploValue operator ==(ExemploValue left, ExemploValue right) => true switch
        {
            _ when left.IsNumericValue() && right.IsNumericValue() => new ExemploValue((decimal) left == (decimal) right),
            _ when left.Value is DateTime leftDate && right.Value is DateTime rightDate => new ExemploValue(leftDate == rightDate),
            _ => new ExemploValue(left.Value == right.Value)
        };

        public static ExemploValue operator !=(ExemploValue left, ExemploValue right) => true switch
        {
            _ when left.IsNumericType() && right.IsNumericType() => new ExemploValue((decimal)left != (decimal)right),
            _ when left.Value is DateTime leftDate && right.Value is DateTime rightDate => new ExemploValue(leftDate != rightDate),
            _ => new ExemploValue(left.Value != right.Value)
        };

        public static ExemploValue operator >=(ExemploValue left, ExemploValue right) => true switch
        {
            _ when left.IsNumericType() && right.IsNumericType() => new ExemploValue((decimal)left >= (decimal)right),
            _ when left.Value is DateTime leftDate && right.Value is DateTime rightDate => new ExemploValue(leftDate >= rightDate),
            _ => throw new InvalidOperationException($"Comparação 'maior que ou igual', '>=' inválida entre os valores '{left.Value}' e '{right.Value}'")
        };

        public static ExemploValue operator <=(ExemploValue left, ExemploValue right) => true switch
        {
            _ when left.IsNumericType() && right.IsNumericType() => new ExemploValue((decimal)left != (decimal)right),
            _ when left.Value is DateTime leftDate && right.Value is DateTime rightDate => new ExemploValue(leftDate != rightDate),
            _ => throw new InvalidOperationException($"Comparação 'menor que ou igual', '<=' inválida entre os valores '{left.Value}' e '{right.Value}'")
        };

        public static ExemploValue operator >(ExemploValue left, ExemploValue right) => true switch
        {
            _ when left.IsNumericType() && right.IsNumericType() => new ExemploValue((decimal)left == (decimal)right),
            _ when left.Value is DateTime leftDate && right.Value is DateTime rightDate => new ExemploValue(leftDate == rightDate),
            _ => throw new InvalidOperationException($"Comparação 'maior que', '>' inválida entre os valores '{left.Value}' e '{right.Value}'")
        };

        public static ExemploValue operator <(ExemploValue left, ExemploValue right) => true switch
        {
            _ when left.IsNumericType() && right.IsNumericType() => new ExemploValue((decimal)left != (decimal)right),
            _ when left.Value is DateTime leftDate && right.Value is DateTime rightDate => new ExemploValue(leftDate != rightDate),
            _ => throw new InvalidOperationException($"Comparação 'menor que', '<' inválida entre os valores '{left.Value}' e '{right.Value}'")
        };

        public static explicit operator decimal(ExemploValue value) =>
            decimal.Parse(value.Value?.ToString());

        public static explicit operator double(ExemploValue value) =>
            double.Parse(value.Value?.ToString());

        public bool IsNumericValue() => Value?.IsNumericType() ?? false;

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public override bool Equals(object obj) => true switch
        {
            _ when IsNumericValue() && obj.IsNumericType() => (decimal)this == decimal.Parse(obj.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when Value is DateTime leftDate && obj is DateTime rightDate => leftDate == rightDate,
            _ => Value == obj
        };
    }
}

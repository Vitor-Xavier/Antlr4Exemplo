using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Antlr4Exemplo.Implementation
{
    public class ExemploVisitor : ExemploBaseVisitor<ExemploValue>
    {
        public IDictionary<string, ExemploValue> _localMemory = new Dictionary<string, ExemploValue>();

        public IDictionary<string, ExemploValue> _externalMemory;

        public ExemploVisitor(IDictionary<string, ExemploValue> externalMemory)
        {
            externalMemory ??= new Dictionary<string, ExemploValue>();
            _externalMemory = externalMemory;
        }

        public override ExemploValue VisitVariableAtom([NotNull] ExemploParser.VariableAtomContext context)
        {
            if (context.GetText() is string key && !string.IsNullOrEmpty(key))
            {
                if (_localMemory.TryGetValue(key, out ExemploValue value))
                    return value;
                else
                    throw new ArgumentException("Variável não declarada");
            }
            else
                throw new ArgumentNullException("Nome da variável não foi informado");
        }

        public override ExemploValue VisitExternalAtom([NotNull] ExemploParser.ExternalAtomContext context)
        {
            if (context.external().GetText() is string key && !string.IsNullOrEmpty(key))
            {
                if (_externalMemory.TryGetValue(key.Replace("@", string.Empty), out ExemploValue value))
                    return value;
                else
                    throw new ArgumentException("Variável externa não encontrada");
            }
            else
                throw new ArgumentNullException("Nome da variável externa não foi informado");
        }

        public override ExemploValue VisitNumberAtom([NotNull] ExemploParser.NumberAtomContext context)
        {
            if (double.TryParse(context.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                return new ExemploValue(number);
            else
                throw new Exception($"Não foi possível converter o valor '{context.GetText()}' em número.");
        }

        public override ExemploValue VisitTextAtom([NotNull] ExemploParser.TextAtomContext context) =>
            new ExemploValue(context.TEXT().GetText()?.Replace("\"", string.Empty));

        public override ExemploValue VisitNullAtom([NotNull] ExemploParser.NullAtomContext context) =>
            new ExemploValue(null);

        public override ExemploValue VisitVariableAssignment([NotNull] ExemploParser.VariableAssignmentContext context)
        {
            string key = context.VARIABLE().GetText();
            var value = Visit(context.expression());
            string op = context.assignment_operator().GetText();

            if (!_localMemory.TryGetValue(key, out ExemploValue currentValue))
                throw new Exception($"Varíavel '{key}' não foi declarada");

            _localMemory[key] = op switch
            {
                "=" => value,
                "+=" when currentValue.Value is string currentString => new ExemploValue(currentString + value.Value?.ToString()),
                "+=" when currentValue.IsNumericValue() && value.IsNumericValue() => currentValue + value,
                "-=" when currentValue.IsNumericValue() && value.IsNumericValue() => currentValue - value,
                "*=" when currentValue.IsNumericValue() && value.IsNumericValue() => currentValue * value,
                "/=" when currentValue.IsNumericValue() && value.IsNumericValue() => currentValue / value,
                _ => throw new ArithmeticException("Atribuição inválida"),
            };
            return _localMemory[key];
        }

        public override ExemploValue VisitVariableDeclaration([NotNull] ExemploParser.VariableDeclarationContext context)
        {
            string key = context.VARIABLE().GetText();
            var value = Visit(context.expression());

            if (_localMemory.ContainsKey(key))
                throw new Exception($"Varíavel '{key}' já foi declarada");

            _localMemory.Add(key, value);

            return value;
        }

        public override ExemploValue VisitParenthesisExpression([NotNull] ExemploParser.ParenthesisExpressionContext context) =>
            Visit(context.arithmetic_expression());

        public override ExemploValue VisitParenthesisComparisonExpression([NotNull] ExemploParser.ParenthesisComparisonExpressionContext context) =>
            Visit(context.comparison_expression());

        public override ExemploValue VisitHighPrecedenceExpression([NotNull] ExemploParser.HighPrecedenceExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));
            string op = context.high_precedence_operator().GetText();

            if (op == "*" && left.IsNumericValue() && right.IsNumericValue())
                return left * right;
            if (op == "/" && left.IsNumericValue() && right.IsNumericValue() && (decimal)right != 0.0m)
                return left / right;

            throw new ArithmeticException($"Não foi executar o comando '{left} {op} {right}'");
        }

        public override ExemploValue VisitLowPrecedenceExpression([NotNull] ExemploParser.LowPrecedenceExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));
            string op = context.low_precedence_operator().GetText();

            if (op == "+" && left.IsNumericValue() && right.IsNumericValue())
                return left + right;
            else if (op == "+" && left.Value is string leftString)
                return new ExemploValue(leftString + right.Value?.ToString());
            if (op == "-" && left.IsNumericValue() && right.IsNumericValue())
                return left - right;

            throw new ArithmeticException($"Não foi executar o comando '{left} {op} {right}'");
        }

        public override ExemploValue VisitPowExpression([NotNull] ExemploParser.PowExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));

            if (left.IsNumericValue() && right.IsNumericValue())
                return new ExemploValue(Math.Pow((double)left, (double) right));
            throw new ArithmeticException("Não foi possível elevar o valor");
        }

        public override ExemploValue VisitCoalesceArithmeticExpression([NotNull] ExemploParser.CoalesceArithmeticExpressionContext context) =>
            Visit(context.null_coalescing_expression());

        public override ExemploValue VisitWhileExpression([NotNull] ExemploParser.WhileExpressionContext context)
        {
            while (bool.Parse(Visit(context.comparison_expression()).Value?.ToString()))
            {
                foreach (var ruleBlock in context.rule_block())
                    Visit(ruleBlock);
            }

            return new ExemploValue(null);
        }

        public override ExemploValue VisitComparisonExpression([NotNull] ExemploParser.ComparisonExpressionContext context)
        {
            var left = Visit(context.arithmetic_expression(0));
            var right = Visit(context.arithmetic_expression(1));
            string op = context.comparison_operator().GetText();

            return op switch
            {
                "==" => left == right,
                "!=" => left != right,
                _ when left.Value is null || right.Value is null => throw new ArgumentNullException("Não é possível fazer a comparação entre valores nulos"),
                ">=" => left >= right,
                "<=" => left <= right,
                ">" => left > right,
                "<" => left < right,
                _ => throw new InvalidOperationException("Operador de comparação inválido")
            };
        }

        public override ExemploValue VisitAndComparisonExpression([NotNull] ExemploParser.AndComparisonExpressionContext context)
        {
            var left = Visit(context.comparison_expression(0));
            var right = Visit(context.comparison_expression(1));

            return new ExemploValue(bool.Parse(left.Value.ToString()) && bool.Parse(right.Value.ToString()));
        }

        public override ExemploValue VisitOrComparisonExpression([NotNull] ExemploParser.OrComparisonExpressionContext context)
        {
            var left = Visit(context.comparison_expression(0));
            var right = Visit(context.comparison_expression(1));

            return new ExemploValue(bool.Parse(left.Value.ToString()) || bool.Parse(right.Value.ToString()));
        }

        public override ExemploValue VisitIfStatement([NotNull] ExemploParser.IfStatementContext context)
        {
            if (Visit(context.comparison_expression(0)).Value is bool value && value)
            {
                Visit(context.if_body());
            }
            else
            {
                if (context.IF(1).GetText() != null && Visit(context.comparison_expression(0)).Value is bool elseValue && elseValue)
                    Visit(context.else_body(0));
                else if (context.else_body(1) != null)
                    Visit(context.else_body(1));
            }

            return new ExemploValue(null);
        }

        public override ExemploValue VisitIfBody([NotNull] ExemploParser.IfBodyContext context)
        {
            foreach (var ruleBlock in context.rule_block())
                Visit(ruleBlock);

            return new ExemploValue(null);
        }

        public override ExemploValue VisitElseBody([NotNull] ExemploParser.ElseBodyContext context)
        {
            foreach (var ruleBlock in context.rule_block())
                Visit(ruleBlock);

            return new ExemploValue(null);
        }

        public override ExemploValue VisitCoalesceExpression([NotNull] ExemploParser.CoalesceExpressionContext context) =>
            new ExemploValue(Visit(context.atom()).Value ?? (context.null_coalescing_expression() != null ? Visit(context.null_coalescing_expression()).Value : null));
    }
}

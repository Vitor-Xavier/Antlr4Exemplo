using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleApp2.Implementation
{
    public class ExemploVisitorFinal : ExemploBaseVisitor<ExemploValue>
    {
        public IDictionary<string, ExemploValue> memory = new Dictionary<string, ExemploValue>();

        public override ExemploValue VisitVariableAtom([NotNull] ExemploParser.VariableAtomContext context)
        {
            if (context.GetText() is string key && !string.IsNullOrEmpty(key))
                if (memory.TryGetValue(key, out ExemploValue value))
                    return value;
                else
                    return new ExemploValue { Value = 0 };
            //throw new Exception($"Variável '{key}' foi não encontrada");
            else
                throw new Exception("Variável não foi informada");
        }

        public override ExemploValue VisitNumberAtom([NotNull] ExemploParser.NumberAtomContext context)
        {
            if (double.TryParse(context.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                return new ExemploValue { Value = number };
            else
                throw new Exception($"Não foi possível converter o valor '{context.GetText()}' em número.");
        }

        public override ExemploValue VisitVariableAssignment([NotNull] ExemploParser.VariableAssignmentContext context)
        {
            string key = context.VARIABLE().GetText();
            var value = Visit(context.expression());

            if (memory.ContainsKey(key))
                throw new Exception($"Varíavel '{key}' já foi declarada");

            memory.Add(key, value);

            return value;
        }

        public override ExemploValue VisitParenthesisExpression([NotNull] ExemploParser.ParenthesisExpressionContext context) =>
            Visit(context.arithmetic_expression());

        public override ExemploValue VisitParenthesisComparisonExpression([NotNull] ExemploParser.ParenthesisComparisonExpressionContext context) =>
            Visit(context.comparison_expression());

        public override ExemploValue VisitPlusExpression([NotNull] ExemploParser.PlusExpressionContext context)
        {
            if (double.TryParse(Visit(context.arithmetic_expression(0))?.Value?.ToString(), out double left) &&
                double.TryParse(Visit(context.arithmetic_expression(1))?.Value?.ToString(), out double right))
                return new ExemploValue { Value = left + right };
            throw new Exception("Erro ao realizar soma");
        }

        public override ExemploValue VisitMinusExpression([NotNull] ExemploParser.MinusExpressionContext context)
        {
            if (double.TryParse(Visit(context.arithmetic_expression(0))?.Value?.ToString(), out double left) &&
                double.TryParse(Visit(context.arithmetic_expression(1))?.Value?.ToString(), out double right))
                return new ExemploValue { Value = left - right };
            throw new Exception("Erro ao realizar subtração");
        }

        public override ExemploValue VisitTimesExpression([NotNull] ExemploParser.TimesExpressionContext context)
        {
            if (double.TryParse(Visit(context.arithmetic_expression(0))?.Value?.ToString(), out double left) &&
                double.TryParse(Visit(context.arithmetic_expression(1))?.Value?.ToString(), out double right))
                return new ExemploValue { Value = left * right };
            throw new Exception("Erro ao realizar multiplicação");
        }

        public override ExemploValue VisitDivExpression([NotNull] ExemploParser.DivExpressionContext context)
        {
            if (double.TryParse(Visit(context.arithmetic_expression(0))?.Value?.ToString(), out double left) &&
                double.TryParse(Visit(context.arithmetic_expression(1))?.Value?.ToString(), out double right) &&
                right != 0.0)
                return new ExemploValue { Value = left / right };
            throw new Exception("Erro ao realizar divisão");
        }
    }
}

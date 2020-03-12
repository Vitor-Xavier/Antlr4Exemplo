//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Exemplo.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ExemploParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IExemploVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExemploParser.rule_set"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRule_set([NotNull] ExemploParser.Rule_setContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExemploParser.rule_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRule_block([NotNull] ExemploParser.Rule_blockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExemploParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] ExemploParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minusExpression</c>
	/// labeled alternative in <see cref="ExemploParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinusExpression([NotNull] ExemploParser.MinusExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>timesExpression</c>
	/// labeled alternative in <see cref="ExemploParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTimesExpression([NotNull] ExemploParser.TimesExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>atomExpression</c>
	/// labeled alternative in <see cref="ExemploParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAtomExpression([NotNull] ExemploParser.AtomExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisExpression</c>
	/// labeled alternative in <see cref="ExemploParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesisExpression([NotNull] ExemploParser.ParenthesisExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>divExpression</c>
	/// labeled alternative in <see cref="ExemploParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDivExpression([NotNull] ExemploParser.DivExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>plusExpression</c>
	/// labeled alternative in <see cref="ExemploParser.arithmetic_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlusExpression([NotNull] ExemploParser.PlusExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesisComparisonExpression</c>
	/// labeled alternative in <see cref="ExemploParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesisComparisonExpression([NotNull] ExemploParser.ParenthesisComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>andComparisonExpression</c>
	/// labeled alternative in <see cref="ExemploParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAndComparisonExpression([NotNull] ExemploParser.AndComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisonExpression</c>
	/// labeled alternative in <see cref="ExemploParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisonExpression([NotNull] ExemploParser.ComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>orComparisonExpression</c>
	/// labeled alternative in <see cref="ExemploParser.comparison_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrComparisonExpression([NotNull] ExemploParser.OrComparisonExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>whileExpression</c>
	/// labeled alternative in <see cref="ExemploParser.loop_expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhileExpression([NotNull] ExemploParser.WhileExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableDeclaration</c>
	/// labeled alternative in <see cref="ExemploParser.variable_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclaration([NotNull] ExemploParser.VariableDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableAssignment</c>
	/// labeled alternative in <see cref="ExemploParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableAssignment([NotNull] ExemploParser.VariableAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifStatement</c>
	/// labeled alternative in <see cref="ExemploParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStatement([NotNull] ExemploParser.IfStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>numberAtom</c>
	/// labeled alternative in <see cref="ExemploParser.atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberAtom([NotNull] ExemploParser.NumberAtomContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>nullAtom</c>
	/// labeled alternative in <see cref="ExemploParser.atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNullAtom([NotNull] ExemploParser.NullAtomContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableAtom</c>
	/// labeled alternative in <see cref="ExemploParser.atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableAtom([NotNull] ExemploParser.VariableAtomContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExemploParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariable([NotNull] ExemploParser.VariableContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExemploParser.comparison_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparison_operator([NotNull] ExemploParser.Comparison_operatorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ExemploParser.assignment_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment_operator([NotNull] ExemploParser.Assignment_operatorContext context);
}
